using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface ISourceService
{
    Task<TResult> AssignAsync(SourceAssignArgs args);
    Task<TResult<object>> AvailablesAsync();
    Task<TResult> CreateAsync(SourceCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<TResult<object>> DetailAsync(int id);
    Task<Source?> FindAsync(int id);
    Task<TResult> GetTypeOfDataBySourceIdAsync(int sourceId);
    Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions);
    Task<object> OptionsAsync(SourceSelectOptions selectOptions);
    Task<object?> OptionsByTypeOfDataAsync(SourceSelectOptions selectOptions);
    Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions);
    Task<object?> TeamOptionsAsync(SourceTeamSelectOptions selectOptions);
    Task<object?> TypeOfDataOptionsAsync(TypeOfDataSelectOptions selectOptions);
    Task<object?> TypeOfDataSourcesAsync();
    Task<TResult> UpdateAsync(SourceUpdateArgs args);
}
