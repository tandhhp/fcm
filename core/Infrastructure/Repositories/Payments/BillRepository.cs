using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Services.Finances.Bills.Filters;
using Waffle.Core.Services.Finances.Interfaces;
using Waffle.Data;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Payments;

public class BillRepository(ApplicationDbContext context) : EfRepository<Bill>(context), IBillRepository
{
    public async Task<decimal> GetRemainingAmountAsync(Guid contractId)
    {
        var totalAmount = await _context.Invoices.Where(x => x.Status == InvoiceStatus.Approved)
            .Where(x => x.ContractId == contractId)
            .SumAsync(x => x.Amount);
        var paidAmount = await _context.Bills.Where(x => x.Status == BillStatus.Approved).SumAsync(x => x.Amount);
        return totalAmount - paidAmount;
    }

    public async Task<ListResult<object>> ListAsync(BillFilterOptions filterOptions)
    {
        var query = from b in _context.Bills
                    join u in _context.Users on b.CreatedBy equals u.Id
                    join c in _context.Contracts on b.ContractId equals c.Id
                    join accountant in _context.Users on b.ApprovedBy equals accountant.Id into accountantJoin
                    from accountant in accountantJoin.DefaultIfEmpty()
                    select new
                    {
                        b.Id,
                        b.BillNumber,
                        CreatedBy = u.Name,
                        b.CreatedDate,
                        b.Status,
                        b.Note,
                        b.Name,
                        ContractCode = c.Code,
                        b.ContractId,
                        b.Amount,
                        b.ApprovedAt,
                        ApprovedBy = accountant.Name
                    };
        if (filterOptions.ContractId.HasValue)
        {
            query = query.Where(x => x.ContractId == filterOptions.ContractId);
        }
        return await ListResult<object>.Success(query, filterOptions);
    }
}
