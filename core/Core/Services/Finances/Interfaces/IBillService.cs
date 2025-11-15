using Waffle.Core.Services.Finances.Bills.Args;
using Waffle.Core.Services.Finances.Bills.Filters;
using Waffle.Models;

namespace Waffle.Core.Services.Finances.Interfaces;

public interface IBillService
{
    Task<TResult> ApproveAsync(Guid id);
    Task<TResult> CreateAsync(BillCreateArgs args);
    Task<TResult> DeleteAsync(Guid id);
    Task<ListResult<object>> ListAsync(BillFilterOptions filterOptions);
    Task<TResult> RejectAsync(Guid id);
    Task<TResult> UpdateAsync(BillUpdateArgs args);
}
