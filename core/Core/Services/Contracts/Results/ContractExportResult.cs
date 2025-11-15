namespace Waffle.Core.Services.Contracts.Results;

public class ContractExportResult
{
    public string? ContractCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public string? CustomerIdNumber { get; set; }
    public string? SalesName { get; set; }
    public string? TOName { get; set; }
    public string? CardName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? AmountPaid { get; set; }
    public decimal? AmountPending { get; set; }
    public string? TeamKeyIn { get; set; }
    public string? KeyIn { get; set; }
    public string? DOS { get; set; }
    public string? SM { get; set; }
    public decimal Discount { get; set; }
}
