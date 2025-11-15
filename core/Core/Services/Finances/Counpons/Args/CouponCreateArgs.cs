namespace Waffle.Core.Services.Finances.Counpons.Args;

public class CouponCreateArgs
{
    public string Name { get; set; } = default!;
    public decimal Discount { get; set; }
    public Guid ContractId { get; set; }
}
