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
    Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions);
    Task<object> OptionsAsync(SelectOptions selectOptions);
    Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions);
    Task<TResult> UpdateAsync(SourceUpdateArgs args);
}
