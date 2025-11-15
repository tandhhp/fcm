using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Core.Services.Finances.Invoices.Filters;

public class InvoiceFilterOptions : FilterOptions
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public InvoiceStatus? Status { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? ContractCode { get; set; }
}
