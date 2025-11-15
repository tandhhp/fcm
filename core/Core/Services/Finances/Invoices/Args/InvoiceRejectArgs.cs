using Waffle.Entities;

namespace Waffle.Core.Services.Finances.Invoices.Args;

public class InvoiceRejectArgs : BaseEntity
{
    public string? Note { get; set; }
}
