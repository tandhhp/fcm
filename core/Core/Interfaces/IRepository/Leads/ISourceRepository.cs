using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Leads;

public interface ISourceRepository : IAsyncRepository<Source>
{
    Task<bool> IsExistAsync(string name);
    Task<bool> IsUsedAsync(int id);
    Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions);
    Task<object> OptionsAsync(SelectOptions selectOptions);
    Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions);
}
