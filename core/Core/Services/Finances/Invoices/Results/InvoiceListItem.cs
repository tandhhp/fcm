using Waffle.Entities;
using Waffle.Entities.Payments;

namespace Waffle.Core.Services.Finances.Invoices.Results;

public class InvoiceListItem : BaseEntity
{
    public string InvoiceNumber { get; set; } = default!;
    public string ContractCode { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public InvoiceStatus Status { get; set; }
    public string? Note { get; set; }
    public string? EvidenceUrl { get; set; }
    public Guid SalesId { get; set; }
    public Guid ContractId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string SalesName { get; set; } = default!;
    public Guid? SaleManagerId { get; set; }
    public Guid? DosId { get; set; }
}
