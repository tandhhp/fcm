using Waffle.Core.Services.Finances.Counpons.Filters;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Finances;

public interface ICouponRepository : IAsyncRepository<Coupon>
{
    Task<ListResult<object>> ListAsync(CouponFilterOptions filterOptions);
}
