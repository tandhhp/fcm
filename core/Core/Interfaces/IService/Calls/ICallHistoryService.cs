using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.ExternalAPI.Tel4vn.Filters;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Calls;

public interface ICallHistoryService
{
    Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions);
    Task<TResult> CompleteAsync(CallCompleteArgs args);
    Task<TResult<object>> StatisticsAsync();
    Task<object?> TeleReportAsync(TeleReportFilterOptions filterOptions);
    Task<ListResult<object>> GetStatusDetailsAsync(CallStatusDetailFilterOptions filterOptions);
}
