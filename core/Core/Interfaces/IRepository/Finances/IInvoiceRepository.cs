using Waffle.Core.Services.Finances.Invoices.Filters;
using Waffle.Core.Services.Finances.Invoices.Results;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Finances;

public interface IInvoiceRepository : IAsyncRepository<Invoice>
{
    Task<List<InvoiceExportListItem>> GetExportListAsync(InvoiceExportFilterOptions args);
    Task<string?> GetLastInvoiceNumberAsync();
    Task<ListResult<InvoiceListItem>> ListAsync(InvoiceFilterOptions filterOptions);
    Task<TResult<object>> StatisticsAsync();
}
