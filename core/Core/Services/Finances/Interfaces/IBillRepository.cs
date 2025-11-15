using Waffle.Core.Interfaces;
using Waffle.Core.Services.Finances.Bills.Filters;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Core.Services.Finances.Interfaces;

public interface IBillRepository : IAsyncRepository<Bill>
{
    Task<decimal> GetRemainingAmountAsync(Guid contractId);
    Task<ListResult<object>> ListAsync(BillFilterOptions filterOptions);
}
