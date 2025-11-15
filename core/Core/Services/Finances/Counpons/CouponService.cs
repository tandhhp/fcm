using Waffle.Core.Interfaces.IRepository.Finances;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Finances;
using Waffle.Core.Services.Finances.Counpons.Args;
using Waffle.Core.Services.Finances.Counpons.Filters;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Services.Finances.Counpons;

public class CouponService(ICouponRepository _couponRepository, IHCAService _hcaService) : ICouponService
{
    public async Task<TResult> CreateAsync(CouponCreateArgs args)
    {
        await _couponRepository.AddAsync(new Coupon
        {
            ContractId = args.ContractId,
            Name = args.Name,
            Discount = args.Discount,
            CreatedBy = _hcaService.GetUserId(),
            CreatedDate = DateTime.Now
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        var coupon = await _couponRepository.FindAsync(id);
        if (coupon is null) return TResult.Failed("Coupon not found");
        await _couponRepository.DeleteAsync(coupon);
        return TResult.Success;
    }

    public Task<ListResult<object>> ListAsync(CouponFilterOptions filterOptions) => _couponRepository.ListAsync(filterOptions);

    public async Task<TResult> UpdateAsync(CouponUpdateArgs args)
    {
        var coupon = await _couponRepository.FindAsync(args.Id);
        if (coupon is null) return TResult.Failed("Coupon not found");
        coupon.Name = args.Name;
        coupon.Discount = args.Discount;
        coupon.ModifiedBy = _hcaService.GetUserId();
        coupon.ModifiedDate = DateTime.Now;
        await _couponRepository.UpdateAsync(coupon);
        return TResult.Success;
    }
}
