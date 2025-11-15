using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Calls;
using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.Infrastructure.Repositories.Calls;
using Waffle.Models;

namespace Waffle.Core.Services.Calls;

public class CallHistoryService(ICallHistoryRepository _callHistoryRepository, IContactService _contactService, IHCAService _hcaService) : ICallHistoryService
{
    public Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions) => _callHistoryRepository.HistoriesAsync(filterOptions);
    public async Task<TResult> CompleteAsync(CallCompleteArgs args)
    {
        try
        {
            var contact = await _contactService.FindAsync(args.ContactId);
            if (contact is null) return TResult.Failed("Không tìm thấy liên hệ!");
            var followUpDate = args.FollowUpDate;
            if (followUpDate.HasValue && args.FollowUpTime.HasValue)
            {
                followUpDate = followUpDate.Value.Date.Add(args.FollowUpTime.Value);
            }
            await _callHistoryRepository.AddAsync(new()
            {
                ContactId = args.ContactId,
                CallStatusId = args.CallStatusId,
                CreatedDate = DateTime.Now,
                Note = args.Note,
                CreatedBy = _hcaService.GetUserId(),
                MetaData = args.MetaData,
                TravelHabit = args.TravelHabit,
                Age = args.Age,
                Job = args.Job,
                ExtraStatus = args.ExtraStatus,
                FollowUpDate = followUpDate
            });
            return TResult.Success;
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    public Task<TResult<object>> StatisticsAsync() => _callHistoryRepository.StatisticsAsync();

    public Task<object?> TeleReportAsync(TeleReportFilterOptions filterOptions) => _callHistoryRepository.TeleReportAsync(filterOptions);

    public Task<ListResult<object>> GetStatusDetailsAsync(CallStatusDetailFilterOptions filterOptions) => _callHistoryRepository.GetStatusDetailsAsync(filterOptions);
}
