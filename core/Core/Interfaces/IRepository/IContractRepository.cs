using Waffle.Core.Services.Contracts.Args;
using Waffle.Core.Services.Contracts.Filters;
using Waffle.Core.Services.Contracts.Results;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IContractRepository : IAsyncRepository<Contract>
{
    Task<bool> AnyAsync(string contractCode);
    Task<ListResult<object>> GetInvoicesAsync(ContractInvoiceFilterOptions filterOptions);
    Task<TResult> CreatePaymentAsync(ContractCreatePayment args, Guid salesId);
    Task<decimal> GetTotalAmountPaidAsync(Guid contractId);
    Task<ListResult<object>> ListAsync(ContractFilterOptions filterOptions);
    Task DeleteInvoicesAsync(Guid id);
    Task<List<ContractExportResult>> GetExportDataAsync(ContractFilterOptions filterOptions);
    Task<TResult> DeleteGiftContractAsync(ContractGiftArgs args);
    Task<ListResult<object>> GetGiftsAsync(ContractGiftFilterOptions filterOptions);
    Task<TResult> GiftContractAsync(ContractGiftArgs args);
    Task DeleteGiftsAsync(Guid id);
    Task<object?> GetLeadOptionsAsync(ContactLeadSelectOptions selectOptions);
}
