using Waffle.Entities.Payments;

namespace Waffle.Core.Services.Finances.Invoices.Filters;

public class InvoiceExportFilterOptions
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public InvoiceStatus? Status { get; set; }
    public string? ContractCode { get; set; }
    public string? InvoiceNumber { get; set; }
}
