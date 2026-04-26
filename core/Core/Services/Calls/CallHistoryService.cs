using System.Globalization;
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
                        Status = LeadStatus.Pending,
                        Confirm2 = Confirm2.UNCONFIRM,
                        SourceId = contact.SourceId
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

    public async Task<TResult> CdrWebhookAsync(CdrWebhookCreateArgs args)
    {
        try
        {
            await _context.CallWebhookLogs.AddAsync(new()
            {
                Application = args.Application,
                Billsec = args.Billsec,
                CallId = args.CallId,
                CampaignUuid = args.CampaignUuid,
                Direction = args.Direction,
                Domain = args.Domain,
                DomainUuid = args.DomainUuid,
                Duration = args.Duration,
                FromNumber = args.FromNumber,
                Hotline = args.Hotline,
                LeadUuid = args.LeadUuid,
                PressKey = args.PressKey,
                ReceiveDest = args.ReceiveDest,
                RecordingUrl = args.RecordingUrl,
                RefId = args.RefId,
                SipCallId = args.SipCallId,
                SipHangupDisposition = args.SipHangupDisposition,
                State = args.State,
                Status = args.Status,
                TimeAnswered = ParseDateTime(args.TimeAnswered),
                TimeEnded = ParseDateTime(args.TimeEnded),
                TimeStarted = ParseDateTime(args.TimeStarted),
                ToNumber = args.ToNumber,
                ReceivedDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return TResult.Success;
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    private static DateTime? ParseDateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            return parsed;
        }

        if (DateTime.TryParse(value, out parsed)) return parsed;
        return null;
    }
}
