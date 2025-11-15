using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Abstractions;
using Waffle.Core.Services.Histories.Models;
using Waffle.Models;
using Waffle.Models.Histories;

namespace Waffle.Core.Interfaces.IService;

public interface ILogService
{
    Task AddAsync(string message, EventLogLevel level = EventLogLevel.LogAlways);
    Task<IdentityResult> DeleteAllAsync();
    Task<IdentityResult> DeleteAsync(Guid id);
    Task ExceptionAsync(Exception ex);
    Task<ListResult<HistoryListItem>> ListAsync(HistoryFilterOptions filterOptions);
}
