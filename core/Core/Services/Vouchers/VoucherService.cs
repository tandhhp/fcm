using OfficeOpenXml;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Vouchers.Args;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Services.Vouchers;

public class VoucherService(IVoucherRepository _voucherRepository, ILogService _logService, ICampaignService _campaignService) : IVoucherService
{
    public async Task<TResult> CreateAsync(VoucherCreateArgs args)
    {
        try
        {
            await _logService.AddAsync($"Tạo mới voucher: {args.Name}");
            var campaign = await _campaignService.FindAsync(args.CampaignId);
            if (campaign == null) return TResult.Failed("Không tìm thấy chiến dịch.");
            await _voucherRepository.AddAsync(new Voucher
            {
                CampaignId = campaign.Id,
                Name = args.Name,
                ExpiredDays = args.ExpiredDays,
                Status = args.Status,
                Note = args.Note
            });
            return TResult.Success;
        }
        catch (Exception ex)
        {
            await _logService.ExceptionAsync(ex);
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        var voucher = await _voucherRepository.FindAsync(id);
        if (voucher == null) return TResult.Failed("Không tìm thấy voucher.");
        if (await _voucherRepository.IsUsedAsync(id)) return TResult.Failed("Voucher đã được sử dụng, không thể xóa.");
        await _logService.AddAsync($"Xóa voucher: {voucher.Name}");
        await _voucherRepository.DeleteAsync(voucher);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var voucher = await _voucherRepository.FindAsync(id);
        if (voucher == null) return TResult<object>.Failed("Không tìm thấy voucher.");
        return TResult<object>.Ok(new
        {
            voucher.Id,
            voucher.CampaignId,
            voucher.Name,
            voucher.ExpiredDays
        });
    }

    public async Task<TResult<byte[]?>> ExportAsync(VoucherFilterOptions filterOptions)
    {
        var data = await _voucherRepository.GetExportDataAsync(filterOptions);
        if (data == null || data.Count == 0) return TResult<byte[]?>.Failed("Không có dữ liệu để xuất.");
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Vouchers");

        worksheet.Cells[1, 1].Value = "STT";
        worksheet.Cells[1, 2].Value = "SDT";
        worksheet.Cells[1, 3].Value = "CCCD";
        worksheet.Cells[1, 4].Value = "Khách hàng";
        worksheet.Cells[1, 5].Value = "Tên Voucher";
        worksheet.Cells[1, 6].Value = "Số ngày hết hạn";
        worksheet.Cells[1, 7].Value = "Loại";
        worksheet.Cells[1, 8].Value = "Trạng thái";
        worksheet.Cells[1, 9].Value = "Ngày kích hoạt";
        worksheet.Cells[1, 10].Value = "Ngày hết hạn";
        worksheet.Cells[1, 11].Value = "Ghi chú";

        var row = 2;
        foreach (var item in data)
        {
            var status1 = EnumHelper.GetDisplayName(item.Status);
            if (item.ActivedAt != null)
            {
                status1 = item.ExpiredAt != null && item.ExpiredAt < DateTime.Now ? "Đã hết hạn" : "Đã kích hoạt";
            }
            worksheet.Cells[row, 1].Value = row - 1;
            worksheet.Cells[row, 2].Value = item.CustomerPhoneNumber;
            worksheet.Cells[row, 3].Value = item.CustomerIdNumber;
            worksheet.Cells[row, 4].Value = item.CustomerName;
            worksheet.Cells[row, 5].Value = item.Code;
            worksheet.Cells[row, 6].Value = item.ExpiredDays;
            worksheet.Cells[row, 7].Value = item.CampaignName;
            worksheet.Cells[row, 8].Value = status1;
            worksheet.Cells[row, 9].Value = item.ActivedAt?.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 10].Value = item.ExpiredAt?.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 11].Value = item.Note;
            row++;
        }
        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        var cells = worksheet.Cells[1, 1, row - 1, 11];
        cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return TResult<byte[]?>.Ok(await package.GetAsByteArrayAsync());
    }

    public Task<Voucher?> FindAsync(Guid id) => _voucherRepository.FindAsync(id);

    public Task<object?> GetOptionsAsync(VoucherSelectOptions selectOptions) => _voucherRepository.GetOptionsAsync(selectOptions);

    public async Task<TResult> ImportAsync(VoucherImportArgs args)
    {
        if (args.File == null || args.File.Length == 0) return TResult.Failed("Vui lòng chọn file để nhập.");
        var campaign = await _campaignService.FindAsync(args.CampaignId);
        if (campaign == null) return TResult.Failed("Không tìm thấy chiến dịch.");
        using var pgk = new ExcelPackage(args.File.OpenReadStream());
        var worksheet = pgk.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;
        var vouchers = new List<Voucher>();
        for (int row = 2; row <= rowCount; row++)
        {
            var name = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(name)) continue;
            var expiredDays = worksheet.Cells[row, 3].GetCellValue<int>();
            vouchers.Add(new Voucher
            {
                CampaignId = campaign.Id,
                Name = name,
                ExpiredDays = expiredDays,
                Status = VoucherStatus.Unused
            });
        }
        if (vouchers.Count == 0) return TResult.Failed("File không có dữ liệu hợp lệ.");
        await _logService.AddAsync($"Nhập khẩu {vouchers.Count} voucher cho chiến dịch: {campaign.Name}");
        await _voucherRepository.AddRangeAsync(vouchers);
        return TResult.Success;
    }

    public Task<bool> IsUsedAsync(Guid id) => _voucherRepository.IsUsedAsync(id);

    public Task<ListResult<object>> ListAsync(VoucherFilterOptions filterOptions) => _voucherRepository.ListAsync(filterOptions);

    public async Task<TResult> UpdateAsync(VoucherUpdateArgs args)
    {
        var voucher = await _voucherRepository.FindAsync(args.Id);
        if (voucher == null) return TResult.Failed("Không tìm thấy voucher.");
        voucher.CampaignId = args.CampaignId;
        voucher.Name = args.Name;
        voucher.ExpiredDays = args.ExpiredDays;
        voucher.Status = args.Status;
        voucher.Note = args.Note;
        if (voucher.Status == VoucherStatus.Unused)
        {
            voucher.ActiveAt = null;
            voucher.ExpiredDate = null;
        }
        await _logService.AddAsync($"Cập nhật voucher: {voucher.Name}");
        await _voucherRepository.UpdateAsync(voucher);
        return TResult.Success;
    }
}
