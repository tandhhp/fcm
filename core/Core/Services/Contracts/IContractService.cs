using Waffle.Core.Services.Contracts.Args;
using Waffle.Core.Services.Contracts.Filters;
using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Services.Contracts;

public interface IContractService
{
    Task<bool> AnyAsync(string contractCode);
    Task<TResult> CreateAsync(ContractCreateArgs args);
    Task<TResult> CreatePaymentAsync(ContractCreatePayment args);
    Task<TResult> DeleteAsync(Guid id);
    Task<TResult> DeleteEvidenceAsync(Guid id);
    Task<TResult<object>> DetailAsync(Guid id);
    Task<TResult<byte[]?>> ExportAsync(ContractFilterOptions filterOptions);
    Task<Contract?> FindAsync(Guid contractId);
    Task<TResult<object>> GetEvidencesAsync(Guid contractId);
    Task<object> GetEvidenceTypeOptionsAsync();
    Task<ListResult<object>> GetInvoicesAsync(ContractInvoiceFilterOptions filterOptions);
    Task<object?> GetLeadOptionsAsync(ContactLeadSelectOptions selectOptions);
    Task<TResult> GiftContractAsync(ContractGiftArgs args);
    Task<ListResult<object>> ListAsync(ContractFilterOptions filterOptions);
    Task<TResult> UpdateAsync(ContractUpdateArgs args);
    Task<TResult> UploadEvidencesAsync(UploadEvidencesArgs args, string host);
}
