using OfficeOpenXml;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository.Finances;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Finances.Invoices.Args;
using Waffle.Core.Services.Finances.Invoices.Filters;
using Waffle.Core.Services.Finances.Invoices.Results;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Core.Services.Finances.Invoices;

public class InvoiceService(IInvoiceRepository _invoiceRepository, ILogService _logService) : IInvoiceService
{
    public async Task<TResult> ApproveAsync(Guid id)
    {
        var invoice = await _invoiceRepository.FindAsync(id);
        if (invoice is null) return TResult.Failed("Không tìm thấy hóa đơn!");
        invoice.Status = InvoiceStatus.Approved;
        await _logService.AddAsync($"Hóa đơn {invoice.InvoiceNumber} đã được duyệt.");
        await _invoiceRepository.UpdateAsync(invoice);
        return TResult.Success;
    }

    public async Task<TResult> CancelAsync(InvoiceCancelArgs args)
    {
        var invoice = await _invoiceRepository.FindAsync(args.Id);
        if (invoice is null) return TResult.Failed("Không tìm thấy hóa đơn!");
        invoice.Status = InvoiceStatus.Cancelled;
        invoice.Note = args.Note;
        await _logService.AddAsync($"Hóa đơn {invoice.InvoiceNumber} đã bị hủy. Lý do: {args.Note}");
        await _invoiceRepository.UpdateAsync(invoice);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var invoice = await _invoiceRepository.FindAsync(id);
        if (invoice is null) return TResult<object>.Failed("Không tìm thấy hóa đơn!");
        return TResult<object>.Ok(new
        {
            invoice.Id,
            invoice.InvoiceNumber,
            invoice.ContractId,
            invoice.SalesId,
            invoice.Amount,
            invoice.PaymentMethod,
            invoice.Status,
            invoice.Note,
            invoice.CreatedAt,
            invoice.CreatedBy
        });
    }

    public async Task<TResult<byte[]?>> ExportAsync(InvoiceExportFilterOptions args)
    {
        try
        {
            var invoices = await _invoiceRepository.GetExportListAsync(args);
            if (invoices.Count == 0) return TResult<byte[]?>.Failed("Không có dữ liệu để xuất!");
            using var pgk = new ExcelPackage();
            var worksheet = pgk.Workbook.Worksheets.Add("Sheet1");

            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Số phiếu thu";
            worksheet.Cells[1, 3].Value = "Hợp đồng";
            worksheet.Cells[1, 4].Value = "Khách hàng";
            worksheet.Cells[1, 5].Value = "Rep";
            worksheet.Cells[1, 6].Value = "SM";
            worksheet.Cells[1, 7].Value = "Số tiền";
            worksheet.Cells[1, 8].Value = "Phương thức thanh toán";
            worksheet.Cells[1, 9].Value = "Ngày thu";
            worksheet.Cells[1, 10].Value = "Trạng thái";
            worksheet.Cells[1, 11].Value = "Người T.O";
            worksheet.Cells[1, 12].Value = "Ghi chú";

            var row = 2;
            foreach (var item in invoices)
            {
                worksheet.Cells[row, 1].Value = row - 1;
                worksheet.Cells[row, 2].Value = item.InvoiceNumber;
                worksheet.Cells[row, 3].Value = item.ContractCode;
                worksheet.Cells[row, 4].Value = item.CustomerName;
                worksheet.Cells[row, 5].Value = item.SalesName;
                worksheet.Cells[row, 6].Value = item.SalesManagerName;
                worksheet.Cells[row, 7].Value = item.Amount;
                worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0";
                worksheet.Cells[row, 8].Value = EnumHelper.GetDisplayName(item.PaymentMethod); ;
                worksheet.Cells[row, 9].Value = item.CreatedAt.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 10].Value = EnumHelper.GetDisplayName(item.Status);
                worksheet.Cells[row, 11].Value = item.TO;
                worksheet.Cells[row, 12].Value = item.Note;
                row++;
            }
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var cells = worksheet.Cells[1, 1, row - 1, 12];

            cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            return TResult<byte[]?>.Ok(await pgk.GetAsByteArrayAsync());
        }
        catch (Exception ex)
        {
            return TResult<byte[]?>.Failed(ex.ToString());
        }
    }

    public async Task<string> GenerateInvoiceNumberAsync()
    {
        var lastInvoiceNumber = await _invoiceRepository.GetLastInvoiceNumberAsync();
        var currentYear = DateTime.Now.Year.ToString().Substring(2); // Lấy 2 chữ số cuối của năm hiện tại
        var newInvoiceNumber = "INV" + currentYear + "0001"; // Mặc định nếu chưa có hóa đơn nào
        if (!string.IsNullOrEmpty(lastInvoiceNumber) && lastInvoiceNumber.Length >= 7)
        {
            var lastYear = lastInvoiceNumber.Substring(3, 2); // Lấy 2 chữ số năm từ hóa đơn cuối cùng
            var lastSequenceStr = lastInvoiceNumber.Substring(5); // Lấy phần số từ hóa đơn cuối cùng
            if (int.TryParse(lastSequenceStr, out int lastSequence))
            {
                if (lastYear == currentYear)
                {
                    // Nếu năm giống nhau, tăng dãy số lên 1
                    newInvoiceNumber = "INV" + currentYear + (lastSequence + 1).ToString("D4");
                }
                else
                {
                    // Nếu năm khác nhau, bắt đầu lại từ 0001
                    newInvoiceNumber = "INV" + currentYear + "0001";
                }
            }
        }
        return newInvoiceNumber;
    }

    public Task<ListResult<InvoiceListItem>> ListAsync(InvoiceFilterOptions filterOptions) => _invoiceRepository.ListAsync(filterOptions);

    public async Task<TResult> RejectAsync(InvoiceRejectArgs args)
    {
        var invoice = await _invoiceRepository.FindAsync(args.Id);
        if (invoice is null) return TResult.Failed("Không tìm thấy hóa đơn!");
        invoice.Status = InvoiceStatus.Rejected;
        invoice.Note = args.Note;
        await _logService.AddAsync($"Hóa đơn {invoice.InvoiceNumber} đã bị từ chối. Lý do: {args.Note}");
        await _invoiceRepository.UpdateAsync(invoice);
        return TResult.Success;
    }

    public Task<TResult<object>> StatisticsAsync() => _invoiceRepository.StatisticsAsync();

    public async Task<TResult> UpdateAsync(InvoiceUpdateArgs args)
    {
        var invoice = await _invoiceRepository.FindAsync(args.Id);
        if (invoice is null) return TResult.Failed("Không tìm thấy hóa đơn!");
        if (invoice.Status != InvoiceStatus.Pending) return TResult.Failed("Chỉ có thể cập nhật các hóa đơn ở trạng thái Chờ duyệt.");
        invoice.Amount = args.Amount;
        invoice.PaymentMethod = args.PaymentMethod;
        invoice.Note = args.Note;
        invoice.InvoiceNumber = args.InvoiceNumber;
        invoice.CreatedAt = args.CreatedAt;
        await _logService.AddAsync($"Hóa đơn {invoice.InvoiceNumber} đã được cập nhật.");
        await _invoiceRepository.UpdateAsync(invoice);
        return TResult.Success;
    }
}
