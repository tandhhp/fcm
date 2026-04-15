using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Contracts;
using Waffle.Core.Services.Events.Args;
using Waffle.Core.Services.Events.Filters;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Events.Results;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Events;

public class EventService(ApplicationDbContext _context, IEventRepository _eventRepository, IContractService _contractService, ILeadService _leadService, ILogService _logService, IHCAService _hcaService, UserManager<ApplicationUser> _userManager, ICampaignService _campaignService) : IEventService
{
    public async Task<TResult> CloseDealAsync(CloseDealArgs args)
    {
        try
        {
            var lead = await _leadService.FindAsync(args.LeadId);
            if (lead is null) return TResult.Failed("Không tìm thấy Key-In!");
            if (lead.Status != LeadStatus.Checkin && lead.Status != LeadStatus.CloseDeal) return TResult.Failed("Trạng thái Key-In không hợp lệ!");
            if (string.IsNullOrWhiteSpace(args.ContractCode)) return TResult.Failed("Vui lòng nhập mã hợp đồng!");
            if (args.ContractAmount <= 0) return TResult.Failed("Số tiền hợp đồng không hợp lệ!");
            if (await _contractService.AnyAsync(args.ContractCode)) return TResult.Failed("Mã hợp đồng đã tồn tại!");
            if (lead.SalesId is null) return TResult.Failed("Bạn chưa chọn Rep!");
            await _eventRepository.CreateContractAsync(lead, args.ContractCode, args.ContractAmount, args.CardId, args.SourceId);
            lead.Status = LeadStatus.CloseDeal;
            _context.Leads.Update(lead);
            await _context.SaveChangesAsync();
            return TResult.Success;
        }
        catch (Exception ex)
        {
            await _logService.ExceptionAsync(ex);
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult> CreateAsync(EventCreateArgs args)
    {
        try
        {
            if (args.CampaignId.HasValue)
            {
                var campaign = await _campaignService.GetAsync(args.CampaignId.Value);
                if (campaign is null) return TResult.Failed("Chiến dịch không tồn tại!");
            }
            await _logService.AddAsync($"Tạo sự kiện {args.Name} vào ngày {args.StartDate.Date.Add(args.StartTime):dd/MM/yyyy HH:mm}");
            await _eventRepository.AddAsync(new Event
            {
                Name = args.Name,
                CreatedBy = _hcaService.GetUserId(),
                CreatedDate = DateTime.Now
            });
            return TResult.Success;
        }
        catch (Exception ex)
        {
            await _logService.ExceptionAsync(ex);
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult> DeleteAsync(int id)
    {
        var data = await _eventRepository.FindAsync(id);
        if (data is null) return TResult.Failed("Sự kiện không tồn tại!");
        await _eventRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var data = await _eventRepository.FindAsync(id);
        if (data is null) return TResult<object>.Failed("Sự kiện không tồn tại!");
        return TResult<object>.Ok(new
        {
            data.Id,
            data.Name
        });
    }

    public Task<ListResult<object>> GetListAsync(EventFilterOptions filterOptions) => _eventRepository.GetListAsync(filterOptions);

    public Task<object> KeyInOptionsAsync(KeyInSelectOptions selectOptions) => _eventRepository.KeyInOptionsAsync(selectOptions);

    public async Task<ListResult<object>> ListAsync(EventFilterOptions filterOptions)
    {
        var query = from a in _context.Events
                    select new
                    {
                        a.Id,
                        a.Name,
                        CreatedBy = _context.Users.Where(x => x.Id == a.CreatedBy).Select(x => x.Name).FirstOrDefault(),
                        a.CreatedDate,
                        a.ModifiedDate,
                        ModifiedBy = _context.Users.Where(x => x.Id == a.ModifiedBy).Select(x => x.Name).FirstOrDefault(),
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderByDescending(x => x.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<ListResult<object>> ListKeyInRevenueAsync(SaleRevenueFilterOptions filterOptions)
    {
        var query = from a in _context.Leads
                    join c in _context.Users on a.SalesId equals c.Id
                    join d in _context.LeadFeedbacks on a.Id equals d.LeadId
                    where a.Status == LeadStatus.CloseDeal
                    select new
                    {
                        a.EventDate,
                        a.EventId,
                        a.CreatedDate,
                        a.Status,
                        a.Note,
                        SaleName = c.Name,
                        SaleUserName = c.UserName,
                        KeyInId = a.Id,
                        KeyInName = a.Name,
                        KeyInPhoneNumber = a.PhoneNumber,
                        a.BranchId,
                        Amount = _context.UserTopups.Where(x => x.LeadId == a.Id && x.Type == TopupType.Event && x.Status == TopupStatus.AccountantApproved).Sum(x => x.Amount),
                        AmountPending = _context.UserTopups.Where(x => x.LeadId == a.Id && x.Type == TopupType.Event && x.Status == TopupStatus.Pending).Sum(x => x.Amount),
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.KeyInName))
        {
            query = query.Where(x => x.KeyInName.ToLower().Contains(filterOptions.KeyInName.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.SaleName))
        {
            query = query.Where(x => x.SaleName.ToLower().Contains(filterOptions.SaleName.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.KeyInPhoneNumber))
        {
            query = query.Where(x => x.KeyInPhoneNumber.ToLower().Contains(filterOptions.KeyInPhoneNumber.ToLower()));
        }
        var user = await _userManager.FindByIdAsync(_hcaService.GetUserId().ToString());
        if (user is null) return ListResult<object>.Failed("User not found");
        query = query.Where(x => x.BranchId == user.BranchId);
        query = query.OrderByDescending(x => x.EventDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> ListSaleRevenueAsync(SaleRevenueFilterOptions filterOptions)
    {
        var query = from a in _context.UserTopups
                    join b in _context.Users on a.SaleId equals b.Id
                    join c in _context.Leads on a.LeadId equals c.Id
                    join d in _context.Users on a.CreatedBy equals d.Id
                    join e in _context.Users on a.AccountantId equals e.Id into e1
                    from e in e1.DefaultIfEmpty()
                    join f in _context.Users on a.DosId equals f.Id into f1
                    where a.Type == TopupType.Event
                    select new
                    {
                        a.Id,
                        a.Amount,
                        a.CreatedDate,
                        a.Status,
                        a.Note,
                        SaleName = b.Name,
                        SaleUserName = b.UserName,
                        KeyInName = c.Name,
                        KeyInPhoneNumber = c.PhoneNumber,
                        CreatedBy = d.Name,
                        c.BranchId,
                        KeyInId = c.Id,
                        AccountantName = e.Name,
                        a.AccountantApprovedDate,
                        a.DirectorApprovedDate,
                    };
        var user = await _context.Users.FindAsync(_hcaService.GetUserId());
        if (user is null) return default;
        if (!string.IsNullOrWhiteSpace(filterOptions.SaleName))
        {
            query = query.Where(x => x.SaleName.ToLower().Contains(filterOptions.SaleName.ToLower()));
        }
        if (_hcaService.IsUserInAnyRole(RoleName.Accountant, RoleName.Dos))
        {
            query = query.Where(x => x.BranchId == user.BranchId);
        }
        if (_hcaService.IsUserInRole(RoleName.Accountant))
        {
            query = query.Where(x => x.Status == TopupStatus.DirectorApproved);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public Task<object> OptionsAsync() => _eventRepository.OptionsAsync();

    public Task<List<SUReportResult>> SuReportAsync(SUFilterOptions filterOptions) => _eventRepository.SuReportAsync(filterOptions);

    public Task<object?> TableOptionsAsync(AllTableFilterOptions filterOptions) => _eventRepository.TableOptionsAsync(filterOptions);

    public Task<object> ToOptionsAsync(SelectOptions selectOptions) => _eventRepository.ToOptionsAsync(selectOptions);

    public async Task<TResult> UpdateAsync(EventUpdateArgs args)
    {
        var data = await _eventRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Sự kiện không tồn tại!");
        await _logService.AddAsync($"Cập nhật sự kiện {data.Name}");
        data.Name = args.Name;
        data.ModifiedBy = _hcaService.GetUserId();
        data.ModifiedDate = DateTime.Now;
        await _eventRepository.UpdateAsync(data);
        return TResult.Success;
    }

    public async Task<TResult<byte[]?>> ExportSuReportAsync(SUFilterOptions filterOptions)
    {
        var data = await _eventRepository.SuReportAsync(filterOptions);
        if (data == null || data.Count == 0) return TResult<byte[]?>.Failed("Không có dữ liệu để xuất.");

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Báo cáo sự kiện");

        // Get all unique attendance names from the data
        var attendanceNames = data
            .SelectMany(x => x.SalesReports)
            .SelectMany(x => x.Attendances)
            .Select(x => x.Name)
            .Distinct()
            .ToList();

        // Create header row
        worksheet.Cells[1, 1].Value = "Sales Manager";
        worksheet.Cells[1, 2].Value = "Sales";
        var col = 3;
        foreach (var attendanceName in attendanceNames)
        {
            worksheet.Cells[1, col].Value = attendanceName;
            col++;
        }
        worksheet.Cells[1, col].Value = "Tổng keyin";
        col++;
        worksheet.Cells[1, col].Value = "Rate";

        // Fill data rows
        var row = 2;
        foreach (var item in data)
        {
            var startRow = row;
            foreach (var salesReport in item.SalesReports)
            {
                worksheet.Cells[row, 1].Value = item.SalesManagerName;
                worksheet.Cells[row, 2].Value = salesReport.SalesName;

                col = 3;
                foreach (var attendanceName in attendanceNames)
                {
                    var attendance = salesReport.Attendances.FirstOrDefault(x => x.Name == attendanceName);
                    worksheet.Cells[row, col].Value = attendance?.Count ?? 0;
                    col++;
                }
                worksheet.Cells[row, col].Value = salesReport.TotalKeyInCount;
                col++;
                worksheet.Cells[row, col].Value = salesReport.TotalRate;
                row++;
            }

            // Merge Sales Manager cells if there are multiple sales under the same manager
            if (item.SalesReports.Count > 1)
            {
                worksheet.Cells[startRow, 1, row - 1, 1].Merge = true;
                worksheet.Cells[startRow, 1, row - 1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
            }
        }

        // Style the header row
        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        // Auto-fit columns
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        // Add borders to all cells
        var totalCols = attendanceNames.Count + 4;
        var cells = worksheet.Cells[1, 1, row - 1, totalCols];
        cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return TResult<byte[]?>.Ok(await package.GetAsByteArrayAsync());
    }
}
