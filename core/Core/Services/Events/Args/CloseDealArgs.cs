namespace Waffle.Core.Services.Events.Args;

public class CloseDealArgs
{
    public Guid LeadId { get; set; }
    public string? ContractCode { get; set; }
    public Guid? CardId { get; set; }
    public decimal ContractAmount { get; set; }
}
