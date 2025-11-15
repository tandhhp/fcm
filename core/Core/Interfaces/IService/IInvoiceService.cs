
using Waffle.Core.Services.Finances.Invoices.Args;
using Waffle.Core.Services.Finances.Invoices.Filters;
using Waffle.Core.Services.Finances.Invoices.Results;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface IInvoiceService
{
    Task<TResult> ApproveAsync(Guid id);
    Task<TResult> CancelAsync(InvoiceCancelArgs args);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<TResult<byte[]?>> ExportAsync(InvoiceExportFilterOptions args);
    Task<string> GenerateInvoiceNumberAsync();
    Task<ListResult<InvoiceListItem>> ListAsync(InvoiceFilterOptions filterOptions);
    Task<TResult> RejectAsync(InvoiceRejectArgs args);
    Task<TResult<object>> StatisticsAsync();
    Task<TResult> UpdateAsync(InvoiceUpdateArgs args);
}
