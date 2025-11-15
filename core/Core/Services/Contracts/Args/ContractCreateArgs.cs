namespace Waffle.Core.Services.Contracts.Args;

public class ContractCreateArgs
{
    public Guid LeadId { get; set; }
    public Guid SalesId { get; set; }
    public decimal Amount { get; set; }
    public Guid ToById { get; set; }
    public string Code { get; set; } = default!;
    public Guid CardId { get; set; }
    public int SourceId { get; set; }
    public Guid? TeamKeyInId { get; set; }
    public Guid? KeyInId { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class ContractUpdateArgs : ContractCreateArgs
{
    public Guid Id { get; set; }
}