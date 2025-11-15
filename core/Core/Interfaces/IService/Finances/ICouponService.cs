using Waffle.Core.Services.Finances.Counpons.Args;
using Waffle.Core.Services.Finances.Counpons.Filters;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Finances;

public interface ICouponService
{
    Task<TResult> CreateAsync(CouponCreateArgs args);
    Task<TResult> DeleteAsync(Guid id);
    Task<ListResult<object>> ListAsync(CouponFilterOptions filterOptions);
    Task<TResult> UpdateAsync(CouponUpdateArgs args);
}
