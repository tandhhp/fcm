namespace Waffle.Core.Services.Contracts.Args;

public class ContractGiftArgs
{
    public string Name { get; set; } = default!;
    public Guid ContractId { get; set; }
    public decimal Amount { get; set; }
    public DateOnly ExpiredDate { get; set; }
}
