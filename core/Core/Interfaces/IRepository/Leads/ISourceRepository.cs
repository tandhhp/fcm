using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Leads;

public interface ISourceRepository : IAsyncRepository<Source>
{
    Task<TResult> AssignAsync(SourceAssignArgs args);
    Task<TResult<object>> AvailablesAsync();
    Task<ListResult<object>> ContactListAsync(SourceContactFilterOptions filterOptions);
    Task<TypeOfData?> GetTypeOfDataByIdAsync(int? typeOfDataId);
    Task<TResult> GetTypeOfDataBySourceIdAsync(int sourceId);
    Task<bool> IsExistAsync(string name);
    Task<bool> IsUsedAsync(int id);
    Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions);
    Task<TResult> MultipleAssignAsync(SourceMultipleAssignArgs args);
    Task<object> OptionsAsync(SourceSelectOptions selectOptions);
    Task<object?> OptionsByTypeOfDataAsync(SourceSelectOptions selectOptions);
    Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions);
    Task<object?> TeamOptionsAsync(SourceTeamSelectOptions selectOptions);
    Task<object?> TypeOfDataOptionsAsync(TypeOfDataSelectOptions selectOptions);
    Task<TResult> TransferAsync(SourceTransferArgs args);
}
