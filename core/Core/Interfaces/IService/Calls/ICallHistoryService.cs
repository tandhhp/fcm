using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.Entities.Contacts;
using Waffle.ExternalAPI.Tel4vn.Filters;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService.Calls;

public interface ICallHistoryService
{
    Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions);
    Task<ListResult<CallWebhookLog>> WebhookLogsAsync(CallWebhookLogFilterOptions filterOptions);
    Task<TResult> CompleteAsync(CallCompleteArgs args);
    Task<TResult> CdrWebhookAsync(CdrWebhookCreateArgs args);
    Task<TResult<byte[]?>> ExportWebhookLogsAsync(CallWebhookLogFilterOptions filterOptions);
    Task<TResult<object>> StatisticsAsync();
    Task<object?> TeleReportAsync(TeleReportFilterOptions filterOptions);
    Task<ListResult<object>> GetStatusDetailsAsync(CallStatusDetailFilterOptions filterOptions);
}
