using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contracts.Args;
using Waffle.Core.Services.Contracts.Filters;
using Waffle.Entities;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Services.Contracts;

public class ContractService(IContractRepository _contractRepository, ILeadService _leadService, ILogService _logService, INotificationService _notificationService, UserManager<ApplicationUser> _userManager, IHCAService _hcaService) : IContractService
{
    public Task<bool> AnyAsync(string contractCode) => _contractRepository.AnyAsync(contractCode);

    public async Task<TResult> CreateAsync(ContractCreateArgs args)
    {
        if (args.Amount <= 0) return TResult.Failed("Số tiền hợp đồng không hợp lệ!");
        if (await AnyAsync(args.Code)) return TResult.Failed("Mã hợp đồng đã tồn tại!");
        var lead = await _leadService.FindAsync(args.LeadId);
        if (lead is null) return TResult.Failed("Không tìm thấy khách hàng!");
        await _contractRepository.AddAsync(new Contract
        {
            Amount = args.Amount,
            CardId = args.CardId,
            CreatedDate = args.CreatedDate ?? DateTime.Now,
            CreatedBy = _hcaService.GetUserId(),
            Code = args.Code,
            PhoneNumber = lead.PhoneNumber,
            SalesId = args.SalesId,
            SourceId = args.SourceId,
            ToById = args.ToById,
            TeamKeyInId = args.TeamKeyInId,
            KeyInId = args.KeyInId
        });
        await _logService.AddAsync($"Hợp đồng {args.Code} đã được tạo");
        return TResult.Success;
    }

    public async Task<TResult> CreatePaymentAsync(ContractCreatePayment args)
    {
        var contract = await _contractRepository.FindAsync(args.ContractId);
        if (contract is null) return TResult.Failed("Không tìm thấy hợp đồng!");
        if (args.Amount <= 0) return TResult.Failed("Số tiền thanh toán không hợp lệ!");
        var amountPaid = await _contractRepository.GetTotalAmountPaidAsync(args.ContractId);
        var remainingAmount = contract.Amount - amountPaid;
        if (args.Amount > remainingAmount) return TResult.Failed("Số tiền thanh toán vượt quá số tiền còn lại của hợp đồng!");
        if (string.IsNullOrWhiteSpace(args.InvoiceNumber)) return TResult.Failed("Số phiếu thu không được để trống!");
        var accountants = await _userManager.GetUsersInRoleAsync(RoleName.Accountant);
        foreach (var accountant in accountants)
        {
            await _notificationService.CreateAsync($"Phiếu thu {args.InvoiceNumber} cần phê duyệt",
                $"Phiếu thu {args.InvoiceNumber} của hợp đồng {contract.Code} cần được phê duyệt",
                accountant.Id);
        }
        if (contract.SalesId is null) return TResult.Failed("Hợp đồng chưa có nhân viên kinh doanh phụ trách!");
        return await _contractRepository.CreatePaymentAsync(args, contract.SalesId.GetValueOrDefault());
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        if (!_hcaService.IsUserInRole(RoleName.Admin)) return TResult.Failed("Bạn không có quyền xóa hợp đồng!");
        var contract = await _contractRepository.FindAsync(id);
        if (contract is null) return TResult.Failed("Không tìm thấy hợp đồng!");
        await _contractRepository.DeleteInvoicesAsync(id);
        await _contractRepository.DeleteGiftsAsync(id);
        await _logService.AddAsync($"Hợp đồng {contract.Code} đã bị xóa");
        await _contractRepository.DeleteAsync(contract);
        return TResult.Success;
    }

    public Task<TResult> DeleteGiftContractAsync(ContractGiftArgs args) => _contractRepository.DeleteGiftContractAsync(args);

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var contract = await _contractRepository.FindAsync(id);
        if (contract is null) return TResult<object>.Failed("Không tìm thấy hợp đồng!");
        var sales = new ApplicationUser();
        if (contract.SalesId.HasValue)
        {
            sales = await _userManager.FindByIdAsync(contract.SalesId.GetValueOrDefault().ToString());
            if (sales is null) return TResult<object>.Failed("Không tìm thấy nhân viên kinh doanh!");
        }
        var lead = await _leadService.FindAsync(contract.LeadId);
        if (lead is null) return TResult<object>.Failed("Không tìm thấy khách hàng!");
        return TResult<object>.Ok(new
        {
            contract.Id,
            contract.Code,
            contract.Amount,
            CustomerName = lead.Name,
            contract.CardId,
            contract.ToById,
            lead.DateOfBirth,
            contract.SalesId,
            contract.PhoneNumber,
            lead.IdentityNumber,
            contract.CreatedDate,
            lead.Gender,
            SalesManagerId = sales.SmId,
            contract.SourceId,
            contract.LeadId,
            contract.KeyInId,
            contract.TeamKeyInId
        });
    }

    public async Task<TResult<byte[]?>> ExportAsync(ContractFilterOptions filterOptions)
    {
        var data = await _contractRepository.GetExportDataAsync(filterOptions);
        if (data.Count == 0) return TResult<byte[]?>.Failed("Không có dữ liệu để xuất!");

        using var pgk = new ExcelPackage();
        var worksheet = pgk.Workbook.Worksheets.Add("Hợp đồng");
        worksheet.Cells[1, 1].Value = "STT";
        worksheet.Cells[1, 2].Value = "Ngày chốt";
        worksheet.Cells[1, 3].Value = "Mã HĐ";
        worksheet.Cells[1, 4].Value = "Khách hàng";
        worksheet.Cells[1, 5].Value = "SDT";
        worksheet.Cells[1, 6].Value = "CCCD";
        worksheet.Cells[1, 7].Value = "Team Keyin";
        worksheet.Cells[1, 8].Value = "Người Keyin";
        worksheet.Cells[1, 9].Value = "DOS";
        worksheet.Cells[1, 10].Value = "SM";
        worksheet.Cells[1, 11].Value = "T.O";
        worksheet.Cells[1, 12].Value = "Rep";
        worksheet.Cells[1, 13].Value = "Loại thẻ";
        worksheet.Cells[1, 14].Value = "GTHĐ";
        worksheet.Cells[1, 15].Value = "GTQĐ";
        worksheet.Cells[1, 16].Value = "GTTT";
        worksheet.Cells[1, 17].Value = "Đã thanh toán";
        worksheet.Cells[1, 18].Value = "Chờ duyệt";
        worksheet.Cells[1, 19].Value = "Tỷ lệ thanh toán";
        worksheet.Cells[1, 20].Value = "Nợ";
        worksheet.Cells[1, 21].Value = "Thời hạn";

        var row = 2;
        foreach (var item in data)
        {
            var amountPaid = item.AmountPaid ?? 0;
            var actualAmount = item.TotalAmount - item.Discount;
            var debt = actualAmount - amountPaid;
            var paymentRate = actualAmount == 0 ? 0 : Math.Round((amountPaid / actualAmount) * 100, 2);
            worksheet.Cells[row, 1].Value = row - 1;
            worksheet.Cells[row, 2].Value = item.CreatedAt.ToString("dd/MM/yyyy");
            worksheet.Cells[row, 3].Value = item.ContractCode;
            worksheet.Cells[row, 4].Value = item.CustomerName;
            worksheet.Cells[row, 5].Value = item.CustomerPhone;
            worksheet.Cells[row, 6].Value = item.CustomerIdNumber;
            worksheet.Cells[row, 7].Value = item.TeamKeyIn;
            worksheet.Cells[row, 8].Value = item.KeyIn;
            worksheet.Cells[row, 9].Value = item.DOS;
            worksheet.Cells[row, 10].Value = item.SM;
            worksheet.Cells[row, 11].Value = item.TOName;
            worksheet.Cells[row, 12].Value = item.SalesName;
            worksheet.Cells[row, 13].Value = item.CardName;
            worksheet.Cells[row, 14].Value = item.TotalAmount;
            worksheet.Cells[row, 14].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 15].Value = item.Discount;
            worksheet.Cells[row, 15].Style.Numberformat.Format = "#,##0"; // Giá trị quy đổi
            worksheet.Cells[row, 16].Value = item.TotalAmount - item.Discount;
            worksheet.Cells[row, 16].Style.Numberformat.Format = "#,##0"; // Giá trị Thực tế
            worksheet.Cells[row, 17].Value = amountPaid;
            worksheet.Cells[row, 17].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 18].Value = item.AmountPending;
            worksheet.Cells[row, 18].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 19].Value = paymentRate + "%";
            worksheet.Cells[row, 20].Value = debt;
            worksheet.Cells[row, 20].Style.Numberformat.Format = "#,##0";
            worksheet.Cells[row, 21].Value = item.TotalAmount != item.AmountPaid ? $"{(item.CreatedAt.AddDays(30) - DateTime.Now).TotalDays:N0} ngày" : "-";
            row++;
        }

        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        var cells = worksheet.Cells[1, 1, row - 1, 21];
        cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return TResult<byte[]?>.Ok(await pgk.GetAsByteArrayAsync());
    }

    public Task<Contract?> FindAsync(Guid id) => _contractRepository.FindAsync(id);

    public Task<ListResult<object>> GetGiftsAsync(ContractGiftFilterOptions filterOptions) => _contractRepository.GetGiftsAsync(filterOptions);

    public Task<ListResult<object>> GetInvoicesAsync(ContractInvoiceFilterOptions filterOptions) => _contractRepository.GetInvoicesAsync(filterOptions);

    public Task<object?> GetLeadOptionsAsync(ContactLeadSelectOptions selectOptions) => _contractRepository.GetLeadOptionsAsync(selectOptions);

    public Task<TResult> GiftContractAsync(ContractGiftArgs args) => _contractRepository.GiftContractAsync(args);

    public Task<ListResult<object>> ListAsync(ContractFilterOptions filterOptions) => _contractRepository.ListAsync(filterOptions);

    public async Task<TResult> UpdateAsync(ContractUpdateArgs args)
    {
        var contract = await _contractRepository.FindAsync(args.Id);
        if (contract is null) return TResult.Failed("Không tìm thấy hợp đồng!");
        if (args.Amount <= 0) return TResult.Failed("Số tiền hợp đồng không hợp lệ!");
        if (contract.Code != args.Code && await AnyAsync(args.Code)) return TResult.Failed("Mã hợp đồng đã tồn tại!");
        contract.Amount = args.Amount;
        contract.Code = args.Code;
        contract.CardId = args.CardId;
        contract.SalesId = args.SalesId;
        contract.SourceId = args.SourceId;
        contract.ToById = args.ToById;
        contract.KeyInId = args.KeyInId;
        contract.TeamKeyInId = args.TeamKeyInId;
        contract.LeadId = args.LeadId;
        contract.CreatedDate = args.CreatedDate ?? contract.CreatedDate;
        await _contractRepository.UpdateAsync(contract);
        await _logService.AddAsync($"Hợp đồng {args.Code} đã được cập nhật");
        return TResult.Success;
    }
}
