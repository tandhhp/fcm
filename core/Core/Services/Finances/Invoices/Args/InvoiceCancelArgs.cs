using Waffle.Entities;

namespace Waffle.Core.Services.Finances.Invoices.Args;

public class InvoiceCancelArgs : BaseEntity
{
    public string? Note { get; set; }
}
