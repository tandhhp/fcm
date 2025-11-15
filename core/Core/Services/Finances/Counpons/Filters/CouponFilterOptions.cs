using Waffle.Models;

namespace Waffle.Core.Services.Finances.Counpons.Filters;

public class CouponFilterOptions : FilterOptions
{
    public Guid? ContractId { get; set; }
}
