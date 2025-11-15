namespace Waffle.Core.Services.Finances.Bills.Args;

public class BillCreateArgs
{
    public string BillNumber { get; set; } = default!;
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public Guid ContractId { get; set; }
    public string Name { get; set; } = default!;
}
