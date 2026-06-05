using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces.IService.Calls;
using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.Core.Services.Calls.Results;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Calls;

public class CallHistoryService(ICallHistoryRepository _callHistoryRepository, ILeadService _leadService, ApplicationDbContext _context, IContactService _contactService, IHCAService _hcaService) : ICallService
{
    public Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions) => _callHistoryRepository.HistoriesAsync(filterOptions);

    public async Task<ListResult<CallWebhookLogListItem>> WebhookLogsAsync(CallWebhookLogFilterOptions filterOptions)
    {
        var query = BuildWebhookLogQuery(filterOptions)
            .OrderByDescending(x => x.ReceivedDate)
            .ThenByDescending(x => x.TimeStarted);
        return await ListResult<CallWebhookLogListItem>.Success(query, filterOptions);
    }

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
                    if (contact.UserId == null) return TResult.Failed("Liên hệ chưa có người phụ trách!");
                    var telesales = await _context.Users.FindAsync(contact.UserId);
                    if (telesales is null) return TResult.Failed("Người phụ trách không tồn tại!");

                    var lead = await _leadService.FindByPhoneNumberAsync(contact.PhoneNumber);
                    if (lead != null)
                    {
                        if (lead.Status == LeadStatus.Checkin) return TResult.Failed($"Khách đã check-in vào ngày {lead.EventDate:dd-MM-yyyy}");
                        if (lead.Status == LeadStatus.CloseDeal) return TResult.Failed($"Khách đã chốt deal vào ngày {lead.EventDate:dd-MM-yyyy}");
                        var leadDetail = await _context.LeadHistories.FirstOrDefaultAsync(x => x.LeadId == lead.Id);
                        await _context.LeadHistories.AddAsync(new LeadHistory
                        {
                            LeadId = lead.Id,
                            EventDate = lead.EventDate,
                            Note = lead.Note,
                            CreatedBy = _hcaService.GetUserId(),
                            AttendanceId = lead.AttendanceId,
                            CheckinTime = leadDetail?.CheckinTime,
                            CheckoutTime = leadDetail?.CheckoutTime,
                            EventId = lead.EventId,
                            SalesId = lead.SalesId,
                            TableId = leadDetail?.TableId,
                            TelesaleId = lead.TelesaleId,
                            ToById = lead.ToById,
                            TransportId = leadDetail?.TransportId
                        });
                        lead.Status = LeadStatus.Pending;
                        lead.EventDate = args.EventDate.GetValueOrDefault();
                        lead.Note = args.Note;
                        lead.EventId = args.EventId.GetValueOrDefault();
                        lead.TelesaleId = contact.UserId;
                        lead.CreatedBy = contact.UserId.GetValueOrDefault();
                        lead.SourceId = contact.SourceId;
                        lead.CreatedDate = DateTime.Now;
                        lead.AppointmentDate = DateTime.Now;
                        lead.Name = contact.Name;
                        lead.BranchId = contact.BranchId.GetValueOrDefault();
                        _context.Leads.Update(lead);
                    }
                    else
                    {
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
                            BranchId = contact.BranchId.GetValueOrDefault(),
                            CreatedBy = contact.UserId.GetValueOrDefault(),
                            Status = LeadStatus.Pending,
                            Confirm2 = Confirm2.UNCONFIRM,
                            SourceId = contact.SourceId,
                            CreatedDate = DateTime.Now,
                            AppointmentDate = DateTime.Now
                        });
                    }
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
            Guid? staffId = null;
            if (!string.IsNullOrEmpty(args.FromNumber))
            {
                var staff = await _context.Users.FirstOrDefaultAsync(x => x.LineCode == args.FromNumber);
                staffId = staff?.Id;
            }
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
                ReceivedDate = DateTime.Now,
                StaffId = staffId
            });
            await _context.SaveChangesAsync();
            return TResult.Success;
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult<byte[]?>> ExportWebhookLogsAsync(CallWebhookLogFilterOptions filterOptions)
    {
        var query = BuildWebhookLogQuery(filterOptions)
            .OrderByDescending(x => x.ReceivedDate)
            .ThenByDescending(x => x.TimeStarted);
        var records = await query.ToListAsync();

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("CallWebhookLogs");

        worksheet.Cells[1, 1].Value = "STT";
        worksheet.Cells[1, 2].Value = "Application";
        worksheet.Cells[1, 3].Value = "Billsec";
        worksheet.Cells[1, 4].Value = "CallId";
        worksheet.Cells[1, 5].Value = "CampaignUuid";
        worksheet.Cells[1, 6].Value = "Direction";
        worksheet.Cells[1, 7].Value = "Domain";
        worksheet.Cells[1, 8].Value = "DomainUuid";
        worksheet.Cells[1, 9].Value = "Duration";
        worksheet.Cells[1, 10].Value = "FromNumber";
        worksheet.Cells[1, 11].Value = "Hotline";
        worksheet.Cells[1, 12].Value = "LeadUuid";
        worksheet.Cells[1, 13].Value = "PressKey";
        worksheet.Cells[1, 14].Value = "ReceiveDest";
        worksheet.Cells[1, 15].Value = "RecordingUrl";
        worksheet.Cells[1, 16].Value = "RefId";
        worksheet.Cells[1, 17].Value = "SipCallId";
        worksheet.Cells[1, 18].Value = "SipHangupDisposition";
        worksheet.Cells[1, 19].Value = "State";
        worksheet.Cells[1, 20].Value = "Status";
        worksheet.Cells[1, 21].Value = "TimeAnswered";
        worksheet.Cells[1, 22].Value = "TimeEnded";
        worksheet.Cells[1, 23].Value = "TimeStarted";
        worksheet.Cells[1, 24].Value = "ToNumber";
        worksheet.Cells[1, 25].Value = "ReceivedDate";

        var row = 2;
        foreach (var item in records)
        {
            worksheet.Cells[row, 1].Value = row - 1;
            worksheet.Cells[row, 2].Value = item.Application;
            worksheet.Cells[row, 3].Value = item.Billsec;
            worksheet.Cells[row, 4].Value = item.CallId;
            worksheet.Cells[row, 5].Value = item.CampaignUuid;
            worksheet.Cells[row, 6].Value = item.Direction;
            worksheet.Cells[row, 7].Value = item.Domain;
            worksheet.Cells[row, 8].Value = item.DomainUuid;
            worksheet.Cells[row, 9].Value = item.Duration;
            worksheet.Cells[row, 10].Value = item.FromNumber;
            worksheet.Cells[row, 11].Value = item.Hotline;
            worksheet.Cells[row, 12].Value = item.LeadUuid;
            worksheet.Cells[row, 13].Value = item.PressKey;
            worksheet.Cells[row, 14].Value = item.ReceiveDest;
            worksheet.Cells[row, 15].Value = item.RecordingUrl;
            worksheet.Cells[row, 16].Value = item.RefId;
            worksheet.Cells[row, 17].Value = item.SipCallId;
            worksheet.Cells[row, 18].Value = item.SipHangupDisposition;
            worksheet.Cells[row, 19].Value = item.State;
            worksheet.Cells[row, 20].Value = item.Status;
            worksheet.Cells[row, 21].Value = item.TimeAnswered?.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 22].Value = item.TimeEnded?.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 23].Value = item.TimeStarted?.ToString("yyyy-MM-dd HH:mm:ss");
            worksheet.Cells[row, 24].Value = item.ToNumber;
            worksheet.Cells[row, 25].Value = item.ReceivedDate.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        var cells = worksheet.Cells[1, 1, Math.Max(1, row - 1), 25];
        cells.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        cells.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

        return TResult<byte[]?>.Ok(await package.GetAsByteArrayAsync());
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

    private IQueryable<CallWebhookLogListItem> BuildWebhookLogQuery(CallWebhookLogFilterOptions filterOptions)
    {
        var query = from cwl in _context.CallWebhookLogs
                    join u in _context.Users on cwl.StaffId equals u.Id into cwlu
                    from u in cwlu.DefaultIfEmpty()
                    select new CallWebhookLogListItem
                    {
                        Id = cwl.Id,
                        Billsec = cwl.Billsec,
                        CampaignUuid = cwl.CampaignUuid,
                        Direction = cwl.Direction,
                        Duration = cwl.Duration,
                        FromNumber = cwl.FromNumber,
                        Hotline = cwl.Hotline,
                        PressKey = cwl.PressKey,
                        ReceiveDest = cwl.ReceiveDest,
                        RecordingUrl = cwl.RecordingUrl,
                        RefId = cwl.RefId,
                        SipCallId = cwl.SipCallId,
                        SipHangupDisposition = cwl.SipHangupDisposition,
                        State = cwl.State,
                        Status = cwl.Status,
                        TimeAnswered = cwl.TimeAnswered,
                        TimeEnded = cwl.TimeEnded,
                        TimeStarted = cwl.TimeStarted,
                        ToNumber = cwl.ToNumber,
                        ReceivedDate = cwl.ReceivedDate,
                        UserName = u.UserName,
                        StaffName = u.Name,
                        StaffId = u.Id,
                        StaffAvatar = u.Avatar,
                        BranchId = u.BranchId
                    };

        if (filterOptions.BillsecFrom.HasValue) query = query.Where(x => x.Billsec >= filterOptions.BillsecFrom);
        if (filterOptions.BillsecTo.HasValue) query = query.Where(x => x.Billsec <= filterOptions.BillsecTo);
        if (!string.IsNullOrWhiteSpace(filterOptions.Direction)) query = query.Where(x => x.Direction != null && x.Direction.Contains(filterOptions.Direction));
        if (filterOptions.DurationFrom.HasValue) query = query.Where(x => x.Duration >= filterOptions.DurationFrom);
        if (filterOptions.DurationTo.HasValue) query = query.Where(x => x.Duration <= filterOptions.DurationTo);
        if (!string.IsNullOrWhiteSpace(filterOptions.FromNumber)) query = query.Where(x => x.FromNumber != null && x.FromNumber.Contains(filterOptions.FromNumber));
        if (!string.IsNullOrWhiteSpace(filterOptions.Hotline)) query = query.Where(x => x.Hotline != null && x.Hotline.Contains(filterOptions.Hotline));
        if (!string.IsNullOrWhiteSpace(filterOptions.LeadUuid)) query = query.Where(x => x.LeadUuid != null && x.LeadUuid.Contains(filterOptions.LeadUuid));
        if (!string.IsNullOrWhiteSpace(filterOptions.PressKey)) query = query.Where(x => x.PressKey != null && x.PressKey.Contains(filterOptions.PressKey));
        if (!string.IsNullOrWhiteSpace(filterOptions.ReceiveDest)) query = query.Where(x => x.ReceiveDest != null && x.ReceiveDest.Contains(filterOptions.ReceiveDest));
        if (!string.IsNullOrWhiteSpace(filterOptions.RecordingUrl)) query = query.Where(x => x.RecordingUrl != null && x.RecordingUrl.Contains(filterOptions.RecordingUrl));
        if (!string.IsNullOrWhiteSpace(filterOptions.SipHangupDisposition)) query = query.Where(x => x.SipHangupDisposition != null && x.SipHangupDisposition.Contains(filterOptions.SipHangupDisposition));
        if (!string.IsNullOrWhiteSpace(filterOptions.Status)) query = query.Where(x => x.Status != null && x.Status.Contains(filterOptions.Status));
        if (filterOptions.TimeAnsweredFrom.HasValue) query = query.Where(x => x.TimeAnswered >= filterOptions.TimeAnsweredFrom);
        if (filterOptions.TimeAnsweredTo.HasValue) query = query.Where(x => x.TimeAnswered <= filterOptions.TimeAnsweredTo);
        if (filterOptions.TimeEndedFrom.HasValue) query = query.Where(x => x.TimeEnded >= filterOptions.TimeEndedFrom);
        if (filterOptions.TimeEndedTo.HasValue) query = query.Where(x => x.TimeEnded <= filterOptions.TimeEndedTo);
        if (filterOptions.TimeStartedFrom.HasValue) query = query.Where(x => x.TimeStarted >= filterOptions.TimeStartedFrom);
        if (filterOptions.TimeStartedTo.HasValue) query = query.Where(x => x.TimeStarted <= filterOptions.TimeStartedTo);
        if (!string.IsNullOrWhiteSpace(filterOptions.ToNumber)) query = query.Where(x => x.ToNumber != null && x.ToNumber.Contains(filterOptions.ToNumber));
        if (filterOptions.ReceivedDateFrom.HasValue) query = query.Where(x => x.ReceivedDate >= filterOptions.ReceivedDateFrom);
        if (filterOptions.ReceivedDateTo.HasValue) query = query.Where(x => x.ReceivedDate <= filterOptions.ReceivedDateTo);

        return query;
    }
}
