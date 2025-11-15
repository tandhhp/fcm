using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Abstractions;
using Waffle.Core.Services.Histories.Models;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Histories;

namespace Waffle.Core.Interfaces.IRepository;

public interface ILogRepository : IAsyncRepository<AppLog>
{
    Task AddAsync(string message, EventLogLevel level);
    Task<IdentityResult> DeleteAllAsync();
    Task<ListResult<HistoryListItem>> ListAsync(HistoryFilterOptions filterOptions);
}
