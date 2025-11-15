using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Finances;
using Waffle.Core.Services.Finances.Counpons.Filters;
using Waffle.Data;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Payments;

public class CouponRepository(ApplicationDbContext context) : EfRepository<Coupon>(context), ICouponRepository
{
    public async Task<ListResult<object>> ListAsync(CouponFilterOptions filterOptions)
    {
        var query = from c in _context.Coupons
                    select new
                    {
                        c.Id,
                        c.Name,
                        c.Discount,
                        c.CreatedDate,
                        c.CreatedBy,
                        c.ModifiedDate,
                        c.ModifiedBy
                    };
        if (filterOptions.ContractId.HasValue)
        {
            query = query.Where(x => x.Id == filterOptions.ContractId);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }
}
