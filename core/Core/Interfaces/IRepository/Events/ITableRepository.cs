using Waffle.Core.Services.Tables.Filters;
using Waffle.Core.Services.Tables.ListResults;
using Waffle.Core.Services.Tables.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Events;

public interface ITableRepository : IAsyncRepository<Table>
{
    Task<TResult<List<AllRoolListItem>>> GetAllTablesAsync(AllTableFilterOptions filterOptions);
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
    Task<ListResult<object>> ListAsync(TableFilterOptions filterOptions);
}
