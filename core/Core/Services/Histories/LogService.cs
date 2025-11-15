using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Abstractions;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Histories.Models;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Histories;

namespace Waffle.Core.Services.Histories;

public class LogService(IWebHostEnvironment _webHostEnvironment, ILogRepository _logRepository, IHCAService _hcaService) : ILogService
{
    public async Task AddAsync(string message, EventLogLevel level = EventLogLevel.LogAlways) => await _logRepository.AddAsync(message, level);

    public Task<IdentityResult> DeleteAllAsync() => _logRepository.DeleteAllAsync();

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var log = await _logRepository.FindAsync(id);
        if (log is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.dataNotFound",
                Description = "Log not found!"
            });
        }
        await _logRepository.DeleteAsync(log);
        return IdentityResult.Success;
    }

    public async Task ExceptionAsync(Exception ex)
    {
        if (_webHostEnvironment.IsDevelopment()) return;
        await _logRepository.AddAsync(new AppLog
        {
            Message = ex.ToString(),
            CreatedDate = DateTime.Now,
            UserName = _hcaService.GetUserName()
        });
    }

    public Task<ListResult<HistoryListItem>> ListAsync(HistoryFilterOptions filterOptions) => _logRepository.ListAsync(filterOptions);
}
