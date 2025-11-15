using Waffle.Entities;
using Waffle.Entities.Payments;

namespace Waffle.Core.Services.Finances.Invoices.Args;

public class InvoiceUpdateArgs : BaseEntity
{
    public string InvoiceNumber { get; set; } = default!;
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}
