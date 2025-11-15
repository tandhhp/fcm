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

    [HttpPost("fetch-wordpress")]
    public async Task<IActionResult> FetchWordPressAsync([FromBody] FetchWordPressArgs args)
    {
        if (string.IsNullOrEmpty(args.Domain)) return BadRequest("No domain found!");
        if (!args.Domain.EndsWith("/"))
        {
            args.Domain += "/";
        }
        if (args.CatalogId != null)
        {
            var parent = await _catalogService.FindAsync(args.CatalogId ?? Guid.Empty);
            if (parent is null) return BadRequest("Catalog not found!");
        }
        var current = 1;
        var editor = await _componentService.GetByNameAsync(nameof(Editor));
        if (editor is null) return BadRequest("Editor not found!");
        while (true)
        {
            var posts = await _wordPressService.ListPostAsync(args.Domain, new Models.SearchFilterOptions
            {
                Current = current
            });
            if (posts is null) break;
            foreach (var post in posts)
            {
                var article = new Catalog
                {
                    Active = true,
                    CreatedBy = User.GetId(),
                    CreatedDate = DateTime.Now,
                    Description = post.Excerpt.Rendered,
                    ModifiedDate = DateTime.Now,
                    Name = post.Title.Rendered ?? string.Empty,
                    NormalizedName = post.Slug ?? string.Empty,
                    ParentId = args.CatalogId,
                    ViewCount = 0,
                    Type = CatalogType.Article,
                    Thumbnail = "/imgs/search-engines-amico.svg"
                };
                await _catalogService.AddAsync(article);
                var content = string.Empty;
                if (!string.IsNullOrEmpty(post.Content.Rendered))
                {
                    content = post.Content.Rendered.Replace("href=\"" + args.Domain, "href=\"" + "/");
                }
                var arguments = new Editor
                {
                    Blocks = new List<BlockEditorBlock>
                    {
                        new BlockEditorBlock
                        {
                            Type = BlockEditorType.RAW,
                            Data = new BlockEditorItemData
                            {
                                Html = post.Content.Rendered
                            }
                        }
                    }
                };
                var work = new WorkContent
                {
                    ComponentId = editor.Id,
                    Active = true,
                    Arguments = JsonSerializer.Serialize(arguments),
                    Name = "WordPress content"
                };
                await _workService.AddAsync(work);
                await _workService.AddItemAsync(work.Id, article.Id);
            }
            current++;
        }
        return Ok(IdentityResult.Success);
    }

    [HttpPost("import"), AllowAnonymous]
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
        
        for (int row = 2; row <= rowCount; row++)
        {
            try
            {
                var eventDate = DateTimeHelper.ParseDateTime(worksheet.Cells[row, 2].GetValue<string>());
                var eventName = worksheet.Cells[row, 3].GetValue<string>();
                var eventId = events.FirstOrDefault(x => x.Name == eventName)?.Id;
                var teleName = worksheet.Cells[row, 5].GetValue<string>();
                var tele = users.FirstOrDefault(x => x.Name == teleName);
                var teamName = worksheet.Cells[row, 6].GetValue<string>();
                var team = teams.FirstOrDefault(x => x.Name == teamName);
                var fullName = worksheet.Cells[row, 9].GetValue<string>();
                var phoneNumber = worksheet.Cells[row, 10].GetValue<string>();
                var dateOfBirth = DateTimeHelper.ParseDateTime(worksheet.Cells[row, 13].GetValue<string>());
                var attendanceName = worksheet.Cells[row, 18].GetValue<string>();
                var attendance = attendances.FirstOrDefault(x => x.Name == attendanceName);
                var sourceName = worksheet.Cells[row, 21].GetValue<string>();
                var source = sources.FirstOrDefault(x => x.Name == sourceName);
                var salesName = worksheet.Cells[row, 22].GetValue<string>();
                var sales = users.FirstOrDefault(x => x.Name == salesName);
                var toName = worksheet.Cells[row, 25].GetValue<string>();
                var to = users.FirstOrDefault(x => x.Name == toName);
                var contractNumber = worksheet.Cells[row, 29].GetValue<string>();
                var contractDate = DateTimeHelper.ParseDateTime(worksheet.Cells[row, 30].GetValue<string>());
                var contractAmount = worksheet.Cells[row, 31].GetValue<decimal>();
                var contractPaid = worksheet.Cells[row, 32].GetValue<decimal>();
                var identityNumber = worksheet.Cells[row, 34].GetValue<string>()?.Trim();
                if (identityNumber?.Length > 12)
                {
                    identityNumber = identityNumber.Substring(0, 12);
                }
                var keyInName = worksheet.Cells[row, 24].GetValue<string>();
                var keyInUser = users.FirstOrDefault(x => x.Name == keyInName);
                var invoiceNumber = worksheet.Cells[row, 42].GetValue<string>();
                var invoiceDate = DateTimeHelper.ParseDateTime(worksheet.Cells[row, 41].GetValue<string>());

                // Create Lead
                var lead = new Lead
                {
                    Id = Guid.NewGuid(),
                    Name = fullName,
                    PhoneNumber = phoneNumber,
                    DateOfBirth = dateOfBirth != DateTime.MinValue ? dateOfBirth : null,
                    SourceId = source?.Id,
                    Status = string.IsNullOrEmpty(contractNumber) ? LeadStatus.Checkin : LeadStatus.LeadAccept,
                    CreatedBy = keyInUser?.Id ?? Guid.Parse("b905393e-8085-4867-9601-08ddf017976b"),
                    CreatedDate = DateTime.Now,
                    AttendanceId = attendance?.Id,
                    BranchId = 1,
                    EventDate = eventDate ?? DateTime.Now,
                    EventId = eventId.GetValueOrDefault(),
                    IdentityNumber = identityNumber
                };

                await _context.Leads.AddAsync(lead);

                await _context.LeadFeedbacks.AddAsync(new LeadFeedback
                {
                    Id = Guid.NewGuid(),
                    LeadId = lead.Id
                });

                // Create Contract if contract number exists
                if (!string.IsNullOrEmpty(contractNumber))
                {
                    var contract = new Contract
                    {
                        Id = Guid.NewGuid(),
                        LeadId = lead.Id,
                        Code = contractNumber,
                        Amount = contractAmount,
                        SourceId = source?.Id,
                        CreatedBy = User.GetId(),
                        CreatedDate = contractDate ?? DateTime.Now,
                        ToById = to?.Id,
                        SalesId = sales?.Id,
                        KeyInId = keyInUser?.Id,
                        PhoneNumber = phoneNumber
                    };

                    await _context.Contracts.AddAsync(contract);

                    var invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        ContractId = contract.Id,
                        Amount = contractPaid,
                        CreatedBy = User.GetId(),
                        CreatedAt = invoiceDate ?? DateTime.Now,
                        Status = InvoiceStatus.Approved,
                        SalesId = sales?.Id ?? Guid.Empty,
                        PaymentMethod = PaymentMethod.Cash,
                        InvoiceNumber = invoiceNumber
                    };
                    await _context.Invoices.AddAsync(invoice);
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
