using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contracts;
using Waffle.Core.Services.Finances.Bills.Args;
using Waffle.Core.Services.Finances.Bills.Filters;
using Waffle.Core.Services.Finances.Interfaces;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Core.Services.Finances.Bills;

public class BillService(IBillRepository _billRepository, IContractService _contractService, IHCAService _hcaService) : IBillService
{
    public async Task<TResult> ApproveAsync(Guid id)
    {
        var bill = await _billRepository.FindAsync(id);
        if (bill is null) return TResult.Failed("Phiếu chi không tồn tại!");
        bill.Status = BillStatus.Approved;
        bill.ApprovedBy = _hcaService.GetUserId();
        bill.ApprovedAt = DateTime.Now;
        await _billRepository.UpdateAsync(bill);
        return TResult.Success;
    }

    public async Task<TResult> CreateAsync(BillCreateArgs args)
    {
        var contract = await _contractService.FindAsync(args.ContractId);
        if (contract is null) return TResult.Failed("Hợp đồng không tồn tại!");
        var remainingAmount = await _billRepository.GetRemainingAmountAsync(args.ContractId);
        if (args.Amount > remainingAmount) return TResult.Failed($"Số tiền phiếu chi vượt quá số tiền còn lại của hợp đồng. Số tiền còn lại: {remainingAmount}");
        var bill = new Bill
        {
            Name = args.Name,
            Note = args.Note,
            Amount = args.Amount,
            BillNumber = args.BillNumber,
            ContractId = args.ContractId,
            CreatedBy = _hcaService.GetUserId(),
            CreatedDate = DateTime.Now,
            Status = BillStatus.Pending
        };
        await _billRepository.AddAsync(bill);
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        var bill = await _billRepository.FindAsync(id);
        if (bill is null) return TResult.Failed("Phiếu chi không tồn tại!");
        await _billRepository.DeleteAsync(bill);
        return TResult.Success;
    }

    public Task<ListResult<object>> ListAsync(BillFilterOptions filterOptions) => _billRepository.ListAsync(filterOptions);

    public async Task<TResult> RejectAsync(Guid id)
    {
        var bill = await _billRepository.FindAsync(id);
        if (bill is null) return TResult.Failed("Phiếu chi không tồn tại!");
        bill.Status = BillStatus.Rejected;
        bill.ApprovedBy = _hcaService.GetUserId();
        bill.ApprovedAt = DateTime.Now;
        await _billRepository.UpdateAsync(bill);
        return TResult.Success;
    }

    public async Task<TResult> UpdateAsync(BillUpdateArgs args)
    {
        var bill = await _billRepository.FindAsync(args.Id);
        if (bill is null) return TResult.Failed("Phiếu chi không tồn tại!");
        if (bill.Status != BillStatus.Pending) return TResult.Failed("Chỉ có thể cập nhật phiếu chi ở trạng thái chờ duyệt!");
        bill.Name = args.Name;
        bill.Note = args.Note;
        bill.Amount = args.Amount;
        bill.BillNumber = args.BillNumber;
        await _billRepository.UpdateAsync(bill);
        return TResult.Success;
    }
}
