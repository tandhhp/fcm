using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Calls;
using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Calls;

public class CallHistoryService(ICallHistoryRepository _callHistoryRepository, ILeadService _leadService, ApplicationDbContext _context, IContactService _contactService, IHCAService _hcaService) : ICallHistoryService
{
    public Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions) => _callHistoryRepository.HistoriesAsync(filterOptions);

    public async Task<TResult> CompleteAsync(CallCompleteArgs args)
    {
        try
        {
            var contact = await _contactService.FindAsync(args.ContactId);
            if (contact is null) return TResult.Failed("Không tìm thấy liên hệ!");
            if (contact.UserId is null) return TResult.Failed("Liên hệ chưa có người phụ trách!");
            var followUpDate = args.FollowUpDate;
            contact.LastCallTime = DateTime.Now;
            contact.ExtraStatus = args.ExtraStatus;
            contact.CallStatusId = args.CallStatusId;
            if (followUpDate.HasValue && args.FollowUpTime.HasValue)
            {
                followUpDate = followUpDate.Value.Date.Add(args.FollowUpTime.Value);
            }
            var callStatus = await _context.CallStatuses.FindAsync(args.CallStatusId);
            if (callStatus is null) return TResult.Failed("Không tìm thấy trạng thái cuộc gọi!");
            if (callStatus.Code == CallStatusCode.CONFIRM1 || callStatus.Code == CallStatusCode.CONSIDER)
            {
                contact.Confirm1 = true;
                if (args.EventDate != null && args.EventId != null)
                {
                    var lead = await _leadService.FindByPhoneNumberAsync(contact.PhoneNumber);
                    if (lead != null && !lead.Duplicated) return TResult.Failed($"Liên hệ đã có lịch hẹn vào ngày {lead.EventDate:dd-MM-yyyy}!");
                    if (contact.UserId == null) return TResult.Failed("Liên hệ chưa có người phụ trách!");
                    var telesales = await _context.Users.FindAsync(contact.UserId);
                    if (telesales is null) return TResult.Failed("Người phụ trách không tồn tại!");
                    await _context.Leads.AddAsync(new Lead
                    {
                        Name = contact.Name,
                        PhoneNumber = contact.PhoneNumber!,
                        Email = contact.Email,
                        EventDate = args.EventDate.GetValueOrDefault(),
                        EventId = args.EventId.GetValueOrDefault(),
                        Gender = contact.Gender,
                        Note = args.Note,
                        TelesaleId = contact.UserId,
                        BranchId = telesales.BranchId,
                        CreatedBy = contact.UserId.GetValueOrDefault(),
                        Status = LeadStatus.Pending
                    });
                }
            }
            contact.FollowUpdate = followUpDate;
            _context.Contacts.Update(contact);
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
