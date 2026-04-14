using Waffle.Core.Services.Contacts.Args;
using Waffle.Core.Services.Contacts.Filters;
using Waffle.Core.Services.Contacts.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;
using Waffle.Models.Filters;

namespace Waffle.Core.Interfaces.IService;

public interface IContactService
{
    Task<TResult> BlockAsync(BlockContactArgs args);
    Task<TResult> CreateContactAsync(CreateContactArgs args);
    Task<Contact?> FindAsync(Guid id);
    Task<TResult<object>> DetailAsync(Guid id);
    public Task<ListResult<object>> GetBlacklistAsync(BlacklistFilterOptions filterOptions);
    Task<TResult> UpdateAsync(ContactUpdateArgs args);
    Task<TResult> CreateAsync(ContactCreateArgs args);
    Task<TResult> BookAsync(ContactBookArgs args);
    Task<ListResult<dynamic>> ListContactAsync(ContactFilterOptions filterOptions);
    Task<TResult> ImportAsync(ContactImportArgs args);
    Task<ListResult<object>> GetUnassignedListAsync(UnassignedFilterOptions filterOptions);
    Task<TResult> AssignSourceAsync(ContactAssignSourceArgs args);
    Task<TResult> Confirm1Async(Guid id);
    Task<TResult> Confirm2Async(UpdateConfirm2Args args);
    Task<ListResult<object>> NeedConfirmsAsync(ContactFilterOptions filterOptions);
    Task<TResult<object>> GetTmrReportAsync();
    Task<ListResult<object>> DialedCallsAsync(ContactFilterOptions filterOptions);
    Task<ListResult<dynamic>> TransferSourceListAsync(ContactTransferFilterOptions filterOptions);
    Task<TResult> TransferSourceBySearchAsync(ContactTransferBySearchArgs args);
    Task<TResult> TransferSourceByCaseAsync(ContactTransferByCaseArgs args);
    Task<TResult> TransferSourceByFileAsync(ContactTransferByFileArgs args);
    Task<TResult> GetReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions);
    Task<TResult<byte[]?>> ExportReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions);
    Task<TResult> GetTmrDataReportAsync(TmrDataReportFilterOptions filterOptions);
    Task<TResult<byte[]?>> ExportTmrDataReportAsync(TmrDataReportFilterOptions filterOptions);
    Task<TResult<byte[]?>> ExportMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions);
    Task<TResult> GetMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions);
}
