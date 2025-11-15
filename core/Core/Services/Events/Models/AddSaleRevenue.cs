namespace Waffle.Core.Services.Events.Models;

public class AddSaleRevenue
{
    public Guid SaleId { get; set; }
    public decimal Amount { get; set; }
    public Guid LeadId { get; set; }
    public string? Note { get; set; }
    public string? ContractCode { get; set; } = default!;
    public List<IFormFile>? Evidences { get; set; }
}
