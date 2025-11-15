using Waffle.Core.Services.Tables.Filters;
using Waffle.Core.Services.Tables.ListResults;
using Waffle.Core.Services.Tables.Models;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Events;

public interface ITableService
{
    Task<TResult> CreateAsync(TableCreateArgs args);
    Task<TResult> DeleteAsync(int id);
    Task<TResult<List<AllRoolListItem>>> GetAllTablesAsync(AllTableFilterOptions filterOptions);
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
    Task<ListResult<object>> ListAsync(TableFilterOptions filterOptions);
    Task<TResult> UpdateAsync(TableUpdateArgs args);
}
