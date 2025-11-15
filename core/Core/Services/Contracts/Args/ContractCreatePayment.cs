using Waffle.Entities.Payments;

namespace Waffle.Core.Services.Contracts.Args;

public class ContractCreatePayment
{
    public Guid ContractId { get; set; }
    public decimal Amount { get; set; }
    public string EvidenceUrl { get; set; } = default!;
    public string? Note { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string InvoiceNumber { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}
