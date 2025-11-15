using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository.Calls;

public interface ICallHistoryRepository : IAsyncRepository<CallHistory>
{
    Task<ListResult<object>> GetStatusDetailsAsync(CallStatusDetailFilterOptions filterOptions);
    Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions);
    Task<TResult<object>> StatisticsAsync();
    Task<object?> TeleReportAsync(TeleReportFilterOptions filterOptions);
}
