using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Text.Json;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Entities.Contracts;
using Waffle.Entities.Payments;
using Waffle.Extensions;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Foundations;
using Waffle.Models.Args;
using Waffle.Models.Components;
using Waffle.Models.Params.Tools;

namespace Waffle.Controllers;

public class ToolController : BaseController
{
    private readonly ICatalogService _catalogService;
    private readonly IWordPressService _wordPressService;
    private readonly IComponentService _componentService;
    private readonly IWorkService _workService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ToolController(ApplicationDbContext context, ICatalogService catalogService, IWordPressService wordPressService, IComponentService componentService, IWorkService workService, UserManager<ApplicationUser> userManager)
    {
        _catalogService = catalogService;
        _context = context;
        _wordPressService = wordPressService;
        _componentService = componentService;
        _workService = workService;
        _userManager = userManager;
    }

    [HttpPost("import"), Authorize(Roles = RoleName.Admin)]
    public async Task<IActionResult> ImportAsync([FromForm] ImportArgs args)
    {
        if (args.File == null || args.File.Length <= 0)
        {
            return BadRequest("No file uploaded.");
        }
        using var package = new ExcelPackage(args.File.OpenReadStream());
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
        if (worksheet == null)
        {
            return BadRequest("No worksheet found in the Excel file.");
        }
        var rowCount = worksheet.Dimension.Rows;
        var events = await _context.Events.ToListAsync();
        var users = await _userManager.Users.ToListAsync();
        var teams = await _context.Teams.ToListAsync();
        var attendances = await _context.Attendances.ToListAsync();
        var sources = await _context.Sources.ToListAsync();
        var contracts = await _context.Contracts.ToListAsync();
        
        var importedCount = 0;
        var errorCount = 0;
        var leads = await _context.Leads.AsNoTracking().ToListAsync();
        var leads1 = new List<Lead>();

        for (int row = 2; row <= rowCount; row++)
        {
            try
            {
                var eventDate = DateTimeHelper.ParseDateTime(worksheet.Cells[row, 1].GetValue<string>());
                var eventName = worksheet.Cells[row, 2].GetValue<string>();
                var eventId = events.FirstOrDefault(x => x.Name == eventName)?.Id;
                var fullName = worksheet.Cells[row, 5].GetValue<string>();
                var phoneNumber = worksheet.Cells[row, 6].GetValue<string>();
                var identityNumber = worksheet.Cells[row, 7].GetValue<string>()?.Trim();
                if (identityNumber?.Length > 12)
                {
                    identityNumber = identityNumber.Substring(0, 12);
                }
                var keyInName = worksheet.Cells[row, 3].GetValue<string>();
                var keyInUser = users.FirstOrDefault(x => x.Name == keyInName);

                var birthYear = worksheet.Cells[row, 11].GetValue<string>();
                DateTime? dateOfBirth = null;
                if (!string.IsNullOrEmpty(birthYear))
                {
                    dateOfBirth = new DateTime(int.Parse(birthYear), 1, 1);
                }

                var attendanceName = worksheet.Cells[row, 12].GetValue<string>();
                var attendance = attendances.FirstOrDefault(x => x.Name == attendanceName);

                var salesName = worksheet.Cells[row, 14].GetValue<string>();
                var rep = users.FirstOrDefault(x => x.Name == salesName);

                var toName = worksheet.Cells[row, 15].GetValue<string>();
                var to = users.FirstOrDefault(x => x.Name == toName);

                var subContactName = worksheet.Cells[row, 8].GetValue<string>();
                var subContactPhone = worksheet.Cells[row, 9].GetValue<string>();
                var subContactIdNumber = worksheet.Cells[row, 10].GetValue<string>()?.Trim();

                if (leads.Any(x => x.IdentityNumber == identityNumber && !string.IsNullOrEmpty(identityNumber)))
                {
                    errorCount++;
                    continue;
                }

                var lead = new Lead
                {
                    Id = Guid.NewGuid(),
                    Name = fullName,
                    PhoneNumber = phoneNumber,
                    DateOfBirth = dateOfBirth,
                    Status = LeadStatus.Checkin,
                    CreatedBy = keyInUser?.Id ?? Guid.Parse("b905393e-8085-4867-9601-08ddf017976b"),
                    CreatedDate = DateTime.Now,
                    AttendanceId = attendance?.Id,
                    BranchId = 1,
                    EventDate = eventDate ?? DateTime.Now,
                    EventId = eventId.GetValueOrDefault(),
                    IdentityNumber = identityNumber,
                    ToById = to?.Id,
                    SalesId = rep?.Id
                };
                leads1.Add(lead);

                await _context.Leads.AddAsync(lead);

                await _context.LeadFeedbacks.AddAsync(new LeadFeedback
                {
                    Id = Guid.NewGuid(),
                    LeadId = lead.Id
                });

                if (!string.IsNullOrWhiteSpace(subContactName))
                {
                    await _context.SubLeads.AddAsync(new SubLead
                    {
                        Id = Guid.NewGuid(),
                        LeadId = lead.Id,
                        Name = subContactName,
                        PhoneNumber = subContactPhone,
                        IdentityNumber = subContactIdNumber
                    });
                }

                importedCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Success = true,
            ImportedCount = importedCount,
            ErrorCount = errorCount,
            Message = $"Successfully imported {importedCount} records. {errorCount} errors."
        });
    }

    [HttpPost("fixed"), AllowAnonymous]
    public async Task<IActionResult> FixedAsync([FromBody] List<FixedImport> args)
    {
        try
        {
            var invoices = await _context.Invoices.ToListAsync();
            foreach (var item in args)
            {
                if (string.IsNullOrEmpty(item.InvoiceNumber) || item.ClosedDate == null) continue;
                var invoice = invoices.FirstOrDefault(x => x.InvoiceNumber == item.InvoiceNumber);
                if (invoice != null)
                {
                    invoice.CreatedAt = item.ClosedDate ?? DateTime.Now;
                    _context.Invoices.Update(invoice);
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}

public class FixedImport
{
    public string? InvoiceNumber { get; set; }
    public DateTime? ClosedDate { get; set; }
}
