using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contacts.Args;
using Waffle.Core.Services.Contacts.Filters;
using Waffle.Core.Services.Contacts.Models;
using Waffle.Core.Services.Leads.Args;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Entities.Payments;
using Waffle.Extensions;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Args;
using Waffle.Models.Filters;

namespace Waffle.Controllers;

public class ContactController(UserManager<ApplicationUser> _userManager,
    INotificationService _notificationService,
    ILogService _appLogService, ApplicationDbContext _context, IContactService _contactService, IHCAService _hcaService, ILeadService _leadService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] ContactFilterOptions filterOptions) => Ok(await _contactService.ListContactAsync(filterOptions));

    [HttpGet("dialed-calls")]
    public async Task<IActionResult> DialedCallsAsync([FromQuery] ContactFilterOptions filterOptions) => Ok(await _contactService.DialedCallsAsync(filterOptions));

    [HttpGet("export-dialed-calls")]
    public async Task<IActionResult> ExportDialedCallsAsync([FromQuery] ContactFilterOptions filterOptions)
    {
        var query = from c in _context.Contacts
                                join ch in _context.CallHistories on c.Id equals ch.ContactId
                                join u in _context.Users on c.UserId equals u.Id
                                join s in _context.Sources on c.SourceId equals s.Id
                                join cs in _context.CallStatuses on ch.CallStatusId equals cs.Id
                                where c.Status != ContactStatus.Blacklisted
                                select new
                                {
                                    c.Id,
                                    c.Name,
                                    c.PhoneNumber,
                                    c.CreatedDate,
                                    c.UserId,
                                    TeleName = u.Name,
                                    CalledAt = ch.CreatedDate,
                                    ch.Note,
                                    SourceName = s.Name,
                                    CallStatusName = cs.Name,
                                    ch.CallStatusId,
                                    ch.Age,
                                    ch.FollowUpDate,
                                    ch.Job,
                                    ch.ExtraStatus
                                };
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(x => x.CalledAt.Date >= filterOptions.FromDate.Value.Date && x.CalledAt.Date <= filterOptions.ToDate.Value.Date);
        }
        query = query.OrderByDescending(x => x.CalledAt);
        var contacts = await query.ToListAsync();
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Dialed Calls");
        // Add headers
        worksheet.Cells[1, 1].Value = "STT";
        worksheet.Cells[1, 2].Value = "Tên liên hệ";
        worksheet.Cells[1, 3].Value = "Số điện thoại";
        worksheet.Cells[1, 4].Value = "Ngày tạo";
        worksheet.Cells[1, 5].Value = "Lần gọi cuối";
        worksheet.Cells[1, 6].Value = "Người gọi";
        worksheet.Cells[1, 7].Value = "Ghi chú lần gọi";
        worksheet.Cells[1, 8].Value = "Nguồn";
        worksheet.Cells[1, 9].Value = "Trạng thái cuộc gọi";
        worksheet.Cells[1, 10].Value = "Tuổi";
        worksheet.Cells[1, 11].Value = "Ngày hẹn gọi lại";
        worksheet.Cells[1, 12].Value = "Công việc";
        worksheet.Cells[1, 13].Value = "Trạng thái mở rộng";

        using (var headerRange = worksheet.Cells[1, 1, 1, 13])
        {
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
        }
        // Add data
        for (int i = 0; i < contacts.Count; i++)
        {
            var contact = contacts[i];
            worksheet.Cells[i + 2, 1].Value = i + 1;
            worksheet.Cells[i + 2, 2].Value = contact.Name;
            worksheet.Cells[i + 2, 3].Value = contact.PhoneNumber;
            worksheet.Cells[i + 2, 4].Value = contact.CreatedDate.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[i + 2, 5].Value = contact.CalledAt.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[i + 2, 6].Value = contact.TeleName;
            worksheet.Cells[i + 2, 7].Value = contact.Note;
            worksheet.Cells[i + 2, 8].Value = contact.SourceName;
            worksheet.Cells[i + 2, 9].Value = contact.CallStatusName;
            worksheet.Cells[i + 2, 10].Value = contact.Age;
            worksheet.Cells[i + 2, 11].Value = contact.FollowUpDate?.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[i + 2, 12].Value = contact.Job;
            worksheet.Cells[i + 2, 13].Value = contact.ExtraStatus;
        }
        worksheet.Cells.AutoFitColumns();
        var excelData = package.GetAsByteArray();
        var fileName = $"Dialed_Calls_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    [HttpGet("need-confirms")]
    public async Task<IActionResult> NeedConfirmsAsync([FromQuery] ContactFilterOptions filterOptions) => Ok(await _contactService.NeedConfirmsAsync(filterOptions));

    [HttpGet("statistics")]
    public async Task<IActionResult> StatisticsAsync()
    {
        var totalContacts = await _context.Contacts.CountAsync();
        var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var previousMonth = currentMonth.AddMonths(-1);
        var totalCurrentMonth = await _context.Contacts.CountAsync(x => x.CreatedDate >= currentMonth);
        var totalPreviousMonth = await _context.Contacts.CountAsync(x => x.CreatedDate >= previousMonth && x.CreatedDate < currentMonth);
        var currentYear = new DateTime(DateTime.Now.Year, 1, 1);
        var totalCurrentYear = await _context.Contacts.CountAsync(x => x.CreatedDate >= currentYear);
        return Ok(TResult<object>.Ok(new
        {
            totalContacts,
            totalCurrentMonth,
            totalPreviousMonth,
            totalCurrentYear
        }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _contactService.DetailAsync(id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact is null) return BadRequest("Không tìm thấy liên hệ!");
        _context.Contacts.Remove(contact);
        var activities = await _context.ContactActivities.Where(x => x.ContactId == id).ToListAsync();
        if (activities.Any())
        {
            _context.ContactActivities.RemoveRange(activities);
        }

        var admin = await _context.Users.FindAsync(User.GetId());
        if (admin is null) return BadRequest("Bạn không có quyền thực hiện điều này!");

        await _appLogService.AddAsync($"{admin.Name} - {admin.UserName} đã xóa liên hệ: {contact.Name} - {contact.PhoneNumber}");
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("activity/list/{id}")]
    public async Task<IActionResult> ListActivityAsync([FromRoute] Guid id)
    {
        var query = _context.ContactActivities.Where(x => x.ContactId == id).OrderByDescending(x => x.CalledDate);
        return Ok(new
        {
            data = await query.ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpPost("activity/add")]
    public async Task<IActionResult> AddActivityAsync([FromBody] ContactActivity args)
    {
        await _context.ContactActivities.AddAsync(args);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("activity/update")]
    public async Task<IActionResult> UpdateActivityAsync([FromBody] ContactActivity args)
    {
        var activity = await _context.ContactActivities.FindAsync(args.Id);
        if (activity is null)
        {
            return BadRequest("Activity not found!");
        }
        activity.Note = args.Note;
        activity.CalledDate = args.CalledDate;
        _context.ContactActivities.Update(activity);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("activity/delete/{id}")]
    public async Task<IActionResult> DeleteActivtyAcync([FromRoute] Guid id)
    {
        var activity = await _context.ContactActivities.FindAsync(id);
        if (activity is null)
        {
            return BadRequest("Activity not found!");
        }
        _context.ContactActivities.Remove(activity);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("lead/add")]
    public async Task<IActionResult> AddLeadAsync([FromBody] LeadCreateArgs args)
    {
        try
        {
            var userId = _hcaService.GetUserId();
            if (string.IsNullOrWhiteSpace(args.PhoneNumber)) return BadRequest("Vui lòng nhập số điện thoại");
            var phoneNumber = args.PhoneNumber.Trim();
            if (!PhoneNumberValidator.IsValidVietnamPhoneNumber(phoneNumber)) return BadRequest("Số điện thoại không hợp lệ");
            var lead = await _leadService.FindByPhoneNumberAsync(phoneNumber);
            if (lead != null && !lead.Duplicated) return BadRequest($"Khách hàng {lead.Name} với SDT {args.PhoneNumber} đã tồn tại, ngày tham gia {lead.EventDate:dd-MM-yyyy}!");
            var status = LeadStatus.Pending;
            if (_hcaService.IsUserInAnyRole(RoleName.Admin, RoleName.SalesManager, RoleName.Dos, RoleName.Event, RoleName.TelesaleManager))
            {
                status = LeadStatus.Approved;
            }
            var newLead = new Lead
            {
                CreatedBy = userId,
                Address = args.Address,
                PhoneNumber = args.PhoneNumber,
                CreatedDate = DateTime.Now,
                Status = status,
                BranchId = args.BranchId,
                Note = args.Note,
                Gender = args.Gender,
                Name = args.Name,
                DateOfBirth = args.DateOfBirth,
                EventDate = args.EventDate,
                EventId = args.EventId,
                Email = args.Email
            };
            if (_hcaService.IsUserInRole(RoleName.Sales))
            {
                var sales = await _context.Users.FindAsync(userId);
                if (sales is null) return BadRequest("Không tìm thấy trợ lý cá nhân!");
                await _notificationService.CreateAsync($"Key-In {args.Name} - {args.PhoneNumber} cần được phê duyệt!", $"Khách hàng {args.Name} - {args.PhoneNumber} đã được tạo mới bởi {sales.Name} cần được bạn phê duyệt!", sales.SmId);
            }
            await _leadService.AddAsync(args);
            return Ok(TResult.Success);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("sublead/remove/{id}")]
    public async Task<IActionResult> RemoveSubLeadAsync([FromRoute] Guid id)
    {
        var subLead = await _context.Leads.FindAsync(id);
        if (subLead is null) return BadRequest("Không tìm thấy khách hàng tiềm năng đi cùng!");
        _context.Leads.Remove(subLead);

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("lead/list")]
    public async Task<IActionResult> ListLeadAsync([FromQuery] LeadFilterOptions filterOptions)
    {
        var user = await _context.Users.FindAsync(User.GetId());
        if (user is null) return BadRequest();
        var query = from a in _context.Leads
                    join b in _context.Users on a.SalesId equals b.Id into ab
                    from b in ab.DefaultIfEmpty()
                    join c in _context.Users on a.TelesaleId equals c.Id into ac
                    from c in ac.DefaultIfEmpty()
                    select new
                    {
                        a.Id,
                        a.CreatedDate,
                        a.Name,
                        a.Email,
                        a.PhoneNumber,
                        a.DateOfBirth,
                        a.EventId,
                        a.Address,
                        a.Status,
                        SalesName = b.Name,
                        b.SmId,
                        b.DosId,
                        a.SalesId,
                        a.EventDate,
                        SaleName = b.Name,
                        a.Gender,
                        a.BranchId,
                        TeleName = c.Name,
                        a.TelesaleId,
                        a.Note,
                        inviteCount = _context.LeadHistories.Count(x => x.LeadId == a.Id) + 1
                    };
        if (User.IsInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.SmId == user.Id);
        }
        if (User.IsInRole(RoleName.Dos))
        {
            query = query.Where(x => x.DosId == user.Id);
        }
        if (User.IsInRole(RoleName.Sales))
        {
            query = query.Where(x => x.SalesId == user.Id);
        }
        if (User.IsInRole(RoleName.Telesale))
        {
            query = query.Where(x => x.TelesaleId == user.Id);
        }
        if (User.IsInRole(RoleName.TelesaleManager))
        {
            var teleIds = await (from a in _context.Users
                                 join b in _context.UserRoles on a.Id equals b.UserId
                                 join c in _context.Roles on b.RoleId equals c.Id
                                 where c.Name == RoleName.Telesale && a.TmId == user.Id
                                 select a.Id).ToListAsync();
            query = query.Where(x => x.TelesaleId != null && teleIds.Contains(x.TelesaleId.Value));
        }
        if (User.IsInRole(RoleName.Dot))
        {
            var tmIds = await (from a in _context.Users
                               join b in _context.UserRoles on a.Id equals b.UserId
                               join c in _context.Roles on b.RoleId equals c.Id
                               where c.Name == RoleName.TelesaleManager && a.DotId == user.Id
                               select a.Id).ToListAsync();

            var teleIds = await (from a in _context.Users
                                 join b in _context.UserRoles on a.Id equals b.UserId
                                 join c in _context.Roles on b.RoleId equals c.Id
                                 where c.Name == RoleName.Telesale && a.TmId != null && tmIds.Contains(a.TmId.Value)
                                 select a.Id).ToListAsync();

            var telesales = await _userManager.GetUsersInRoleAsync(RoleName.Telesale);

            query = query.Where(x => x.TelesaleId != null && teleIds.Contains(x.TelesaleId.Value));
        }
        if (!string.IsNullOrEmpty(filterOptions.PhoneNumber))
        {
            query = query.Where(x => x.PhoneNumber == filterOptions.PhoneNumber);
        }
        if (!string.IsNullOrEmpty(filterOptions.Email))
        {
            query = query.Where(x => x.Email == filterOptions.Email);
        }
        if (!string.IsNullOrEmpty(filterOptions.Name))
        {
            query = query.Where(x => x.Name == filterOptions.Name);
        }
        if (filterOptions.EventDate != null)
        {
            query = query.Where(x => x.EventDate.Date == filterOptions.EventDate.Value.Date);
        }
        if (filterOptions.FromDate != null && filterOptions.ToDate != null)
        {
            query = query.Where(x => x.EventDate.Date >= filterOptions.FromDate.Value.Date && x.EventDate <= filterOptions.ToDate.Value.Date);
        }
        if (!User.IsInRole(RoleName.Telesale) && !User.IsInRole(RoleName.TelesaleManager) && !User.IsInRole(RoleName.Dot) && !User.IsInRole(RoleName.Admin) && !User.IsInRole(RoleName.CxTP))
        {
            query = query.Where(x => x.BranchId == user.BranchId);
        }
        if (filterOptions.BranchId != null)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId);
        }
        if (filterOptions.EventId != null)
        {
            query = query.Where(x => x.EventId == filterOptions.EventId);
        }
        if (filterOptions.SmId != null)
        {
            query = query.Where(x => x.SmId == filterOptions.SmId);
        }

        return Ok(new
        {
            data = await query.OrderByDescending(x => x.EventDate).ThenBy(x => x.EventId).Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpGet("subleads/{id}")]
    public async Task<IActionResult> ListSubLeadAsync([FromRoute] Guid id)
    {
        var query = _context.SubLeads.Where(x => x.LeadId == id);
        return Ok(new
        {
            data = await query.ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpPost("lead/delete/{id}")]
    public async Task<IActionResult> DeleteLeadAsync([FromRoute] Guid id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return BadRequest();
        if (!_hcaService.IsUserInAnyRole(RoleName.Admin, RoleName.Event)) return BadRequest("Bạn không có quyền!");
        if (await _context.Contracts.AnyAsync(x => x.LeadId == lead.Id))
        {
            return BadRequest("Khách hàng đã phát sinh hợp đồng, không thể xóa!");
        }
        var subleads = await _context.SubLeads.Where(x => x.LeadId == id).ToListAsync();
        if (subleads.Count != 0)
        {
            _context.SubLeads.RemoveRange(subleads);
        }
        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("lead-options")]
    public async Task<IActionResult> LeadOptionsAsync([FromQuery] Guid eventId) => Ok(await _context.Leads
        .Where(x => x.Status == LeadStatus.Approved)
        .Where(x => x.EventId == eventId).Select(x => new
        {
            label = x.Name,
            value = x.Id
        }).ToListAsync());

    [HttpGet("event/{id}")]
    public async Task<IActionResult> GetEventAsync([FromRoute] Guid id) => Ok(await _context.Events.FindAsync(id));

    [HttpGet("lead/{id}")]
    public async Task<IActionResult> GetLeadDetailAsync([FromRoute] Guid id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead is null) return BadRequest("Không tìm thấy khách hàng tiềm năng");
        var evt = await _context.Events.FindAsync(lead.EventId);
        if (evt is null) return BadRequest("Sự kiện không tồn tại");
        return Ok(new
        {
            data = new
            {
                lead.Id,
                lead.EventDate,
                lead.EventId,
                lead.Name,
                lead.Status,
                lead.Address,
                lead.Email,
                lead.BranchId,
                lead.CreatedDate,
                lead.DateOfBirth,
                lead.Gender,
                lead.IdentityNumber,
                lead.PhoneNumber,
                lead.SalesId,
                EventName = evt.Name,
                SubLeads = await _context.SubLeads.Where(x => x.LeadId == lead.Id).ToListAsync()
            }
        });
    }

    [HttpGet("users-in-event")]
    public async Task<IActionResult> UsersInEventAsync([FromQuery] LeadFilterOptions filterOptions)
    {
        try
        {
            var user = await _context.Users.FindAsync(User.GetId());
            if (user is null) return Unauthorized();
            var query = from a in _context.Leads
                        join c in _context.Users on a.CreatedBy equals c.Id into ac
                        from c in ac.DefaultIfEmpty()
                        join d in _context.Users on a.ToById equals d.Id into ad
                        from d in ad.DefaultIfEmpty()
                        join e in _context.Users on a.SalesId equals e.Id into ae
                        from e in ae.DefaultIfEmpty()
                        where a.Status == LeadStatus.Approved || a.Status == LeadStatus.Pending
                        select new
                        {
                            a.Id,
                            a.EventDate,
                            a.Gender,
                            a.CreatedDate,
                            a.Name,
                            a.EventId,
                            a.Address,
                            a.BranchId,
                            a.Status,
                            a.PhoneNumber,
                            a.DateOfBirth,
                            a.Email,
                            a.SalesId,
                            SalesName = _context.Users.First(x => x.Id == a.SalesId).Name,
                            a.AttendanceId,
                            a.TelesaleId,
                            a.Note,
                            TelesalesName = _context.Users.First(x => x.Id == a.TelesaleId).Name,
                            inviteCount = _context.LeadHistories.Count(x => x.LeadId == a.Id) + 1,
                            a.SourceId,
                            CreatedByName = c.Name,
                            SourceName = _context.Sources.First(x => x.Id == a.SourceId).Name,
                            ToName = d.Name,
                            a.CreatedBy,
                            c.DosId,
                            c.DotId,
                            e.SmId,
                            c.TmId,
                            SubLeads = _context.SubLeads.Where(x => x.LeadId == a.Id).Select(x => $"{x.Name} - {x.PhoneNumber}").ToList()
                        };
            if (filterOptions.EventDate != null)
            {
                query = query.Where(x => x.EventDate.Date == filterOptions.EventDate.Value.Date);
            }
            if (filterOptions.Status != null)
            {
                query = query.Where(x => x.Status == filterOptions.Status);
            }
            if (!string.IsNullOrWhiteSpace(filterOptions.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
            }
            if (!string.IsNullOrEmpty(filterOptions.Email))
            {
                query = query.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(filterOptions.Email.ToLower()));
            }
            if (filterOptions.SourceId.HasValue)
            {
                query = query.Where(x => x.SourceId == filterOptions.SourceId);
            }
            if (filterOptions.EventId.HasValue)
            {
                query = query.Where(x => x.EventId == filterOptions.EventId);
            }
            if (User.IsInRole(RoleName.Sales))
            {
                query = query.Where(x => x.SalesId == user.Id || x.CreatedBy == user.Id);
            }
            if (User.IsInRole(RoleName.Telesale))
            {
                query = query.Where(x => x.TelesaleId == user.Id || x.CreatedBy == user.Id);
            }
            if (User.IsInRole(RoleName.SalesManager))
            {
                query = query.Where(x => x.SmId == user.Id || x.CreatedBy == user.Id);
            }
            if (User.IsInRole(RoleName.Dos))
            {
                query = query.Where(x => x.DosId == user.Id || x.CreatedBy == user.Id);
            }

            query = query.OrderByDescending(x => x.EventDate);
            return Ok(await ListResult<object>.Success(query, filterOptions));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("feedback/{id}")]
    public async Task<IActionResult> GetFeedbackAsync([FromRoute] Guid id)
    {
        var feedback = await _context.LeadFeedbacks.FirstOrDefaultAsync(x => x.LeadId == id);
        feedback ??= new LeadFeedback();
        var invoice = new Invoice();
        var lead = await _context.Leads.FindAsync(id);
        if (lead is null) return BadRequest("Không tìm thấy khách hàng tiềm năng");
        var toName = string.Empty;
        if (lead.ToById != null)
        {
            var to = await _context.Users.FindAsync(lead.ToById);
            if (to != null)
            {
                toName = to.Name;
            }
        }
        return Ok(new
        {
            feedback.Id,
            feedback.LeadId,
            feedback.InterestLevel,
            feedback.FinancialSituation,
            feedback.RejectReason,
            feedback.CheckoutTime,
            feedback.CheckinTime,
            feedback.JobKindId,
            feedback.TableId,
            lead.AttendanceId,
            invoice?.EvidenceUrl,
            invoice?.Amount,
            lead.SalesId,
            lead.SourceId,
            lead.IdentityNumber,
            lead.TelesaleId,
            toName,
            lead.ToById,
            feedback.TransportId
        });
    }

    [HttpPost("feedback/add")]
    public async Task<IActionResult> AddFeedbackAsync([FromBody] LeadFeedbackCreateArgs args)
    {
        if (await _context.LeadFeedbacks.AnyAsync(x => x.LeadId == args.LeadId))
        {
            return BadRequest("Feedback đã được thêm, vui lòng tải lại trang!");
        }
        var lead = await _context.Leads.FindAsync(args.LeadId);
        if (lead == null) return BadRequest("Không tìm thấy khách hàng tiềm năng");
        await _context.LeadFeedbacks.AddAsync(new LeadFeedback
        {
            CheckinTime = args.CheckinTime,
            CheckoutTime = args.CheckoutTime,
            FinancialSituation = args.FinancialSituation,
            InterestLevel = args.InterestLevel,
            JobKindId = args.JobKindId,
            LeadId = args.LeadId,
            TableId = args.TableId,
            TransportId = args.TransportId
        });
        lead.SalesId = args.SalesId;
        lead.SourceId = args.SourceId;
        lead.IdentityNumber = args.IdentityNumber;
        lead.Status = LeadStatus.Checkin;
        _context.Leads.Update(lead);

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("feedback/update")]
    public async Task<IActionResult> FeedbackUpdateAsync([FromBody] LeadFeedbackUpdateArgs args)
    {
        try
        {
            var data = await _context.LeadFeedbacks.FirstOrDefaultAsync(x => x.LeadId == args.LeadId);
            if (data is null)
            {
                data = new LeadFeedback
                {
                    LeadId = args.LeadId
                };
                await _context.LeadFeedbacks.AddAsync(data);
                await _context.SaveChangesAsync();
            }

            var lead = await _context.Leads.FindAsync(args.LeadId);
            if (lead == null) return BadRequest("Không tìm thấy Key-In");
            if (lead.Status == LeadStatus.LeadReject) return BadRequest("Không thể chỉnh sửa khách hàng đã chuyển đổi hoặc khách hàng đã từ chối");
            data.InterestLevel = args.InterestLevel;
            data.FinancialSituation = args.FinancialSituation;
            data.RejectReason = args.RejectReason;
            lead.ToById = args.ToById;
            data.JobKindId = args.JobKindId;
            data.CheckoutTime = args.CheckoutTime;
            data.TransportId = args.TransportId;
            data.TableId = args.TableId;
            lead.SourceId = args.SourceId;
            _context.Leads.Update(lead);
            _context.LeadFeedbacks.Update(data);

            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("sublead/add")]
    public async Task<IActionResult> AddSubLeadAsync([FromBody] SubLead args)
    {
        var phoneNumber = args.PhoneNumber;
        if (!string.IsNullOrWhiteSpace(args.PhoneNumber))
        {
            phoneNumber = args.PhoneNumber.Trim();
            if (phoneNumber.Length != 10)
            {
                return BadRequest("Số điện thoại không hợp lệ");
            }
            if (await _context.SubLeads.AnyAsync(x => x.PhoneNumber == phoneNumber))
            {
                return BadRequest("Số điện thoại đã tồn tại");
            }
        }
        args.PhoneNumber = phoneNumber;
        await _context.SubLeads.AddAsync(args);

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("sublead/update")]
    public async Task<IActionResult> UpdateSubLeadAsync([FromBody] SubLead args)
    {
        var sublead = await _context.SubLeads.FindAsync(args.Id);
        if (sublead is null) return BadRequest("Không tìm thấy người đi cùng");
        var phoneNumber = args.PhoneNumber;
        if (!string.IsNullOrWhiteSpace(args.PhoneNumber))
        {
            if (!PhoneNumberValidator.IsValidVietnamPhoneNumber(args.PhoneNumber)) return BadRequest("Số điện thoại không hợp lệ");
        }
        sublead.PhoneNumber = phoneNumber;
        sublead.IdentityNumber = args.IdentityNumber;
        sublead.Address = args.Address;
        sublead.Gender = args.Gender;
        sublead.Name = args.Name;
        _context.SubLeads.Update(sublead);

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("sublead/delete/{id}")]
    public async Task<IActionResult> DeleteSubLeadAsync([FromRoute] Guid id)
    {
        var sublead = await _context.SubLeads.FindAsync(id);
        if (sublead is null) return BadRequest("Sub Lead not found!");
        _context.SubLeads.Remove(sublead);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("floor-options")]
    public async Task<IActionResult> GetFloorOptionsAsync() => Ok(await _context.Tables.Select(x => x.Floor).Distinct().Select(x => new
    {
        label = x,
        value = x
    }).ToListAsync());

    [HttpGet("table-options")]
    public async Task<IActionResult> GetTableOptionsAsync([FromQuery] DateTime? eventDate)
    {
        var user = await _userManager.FindByIdAsync(User.GetClaimId());
        if (user is null) return Unauthorized();
        var tables = await _context.Tables
        .OrderBy(x => x.SortOrder).AsNoTracking().ToListAsync();

        eventDate ??= DateTime.Now;
        var tableStatuses = await _context.EventTables.Where(x => x.EventDate.Date == eventDate.Value.Date).AsNoTracking().ToListAsync();

        return Ok(tables.Select(x => new
        {
            label = x.Name,
            value = x.Id,
            disabled = tableStatuses.Any(t => t.TableId == x.Id)
        }));
    }

    [HttpGet("my-keyin-list")]
    public async Task<IActionResult> GetMyKeyInAsync([FromQuery] UserFilterOptions filterOptions)
    {
        var userId = User.GetId();
        var query = from a in _context.Leads
                    join b in _context.Users on a.SalesId equals b.Id into ab
                    from b in ab.DefaultIfEmpty()
                    join c in _context.Users on a.TelesaleId equals c.Id into ac
                    from c in ac.DefaultIfEmpty()
                    select new
                    {
                        a.Id,
                        a.EventDate,
                        a.EventId,
                        a.CreatedDate,
                        a.Gender,
                        a.Name,
                        a.Address,
                        a.BranchId,
                        a.DateOfBirth,
                        a.Email,
                        a.PhoneNumber,
                        a.TelesaleId,
                        a.Status,
                        a.SalesId,
                        SaleName = b.Name,
                        TeleName = c.Name,
                        inviteCount = _context.LeadHistories.Count(x => x.LeadId == a.Id) + 1
                    };
        if (filterOptions.BranchId != null)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (!User.IsInRole(RoleName.Event))
        {
            query = query.Where(x => x.TelesaleId == userId || x.SalesId == userId);
        }

        return Ok(new
        {
            data = await query.OrderByDescending(x => x.EventDate).Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpPost("reinvite")]
    public async Task<IActionResult> ReinviteAsync([FromBody] Lead args)
    {
        try
        {
            var lead = await _context.Leads.FindAsync(args.Id);
            if (lead is null) return BadRequest("Không tìm thấy khách hàng!");
            if (lead.Status == LeadStatus.LeadAccept) return BadRequest("Khách đã chốt deal không thể mời lại!");

            var feedback = await _context.LeadFeedbacks.FirstOrDefaultAsync(x => x.LeadId == lead.Id);
            lead.Status = LeadStatus.ReInvite;
            await _context.LeadHistories.AddAsync(new LeadHistory
            {
                LeadId = lead.Id,
                EventDate = lead.EventDate,
                Note = lead.Note,
                SalesId = lead.SalesId,
                TelesaleId = lead.TelesaleId,
                AttendanceId = lead.AttendanceId,
                ToById = lead.ToById,
                CreatedBy = lead.CreatedBy,
                CheckinTime = feedback?.CheckinTime,
                CheckoutTime = feedback?.CheckoutTime,
                TableId = feedback?.TableId,
                TransportId = feedback?.TransportId,
                EventId = lead.EventId
            });
            if (feedback != null)
            {
                feedback.TableId = null;
                feedback.CheckinTime = null;
                feedback.CheckoutTime = null;
                feedback.InterestLevel = 0;
                feedback.FinancialSituation = null;
                feedback.RejectReason = string.Empty;
                _context.LeadFeedbacks.Update(feedback);
            }
            lead.SalesId = null;
            lead.ToById = null;
            lead.EventDate = args.EventDate;
            lead.EventId = args.EventId;
            lead.Note = args.Note;
            _context.Leads.Update(lead);

            await _context.SaveChangesAsync();

            return Ok(TResult.Success);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("checkout-table")]
    public async Task<IActionResult> CheckoutTableAysnc([FromBody] EventTable args)
    {
        var data = await _context.EventTables.FirstOrDefaultAsync(x => x.EventDate.Date == args.EventDate.Date && x.TableId == args.TableId && x.EventId == args.EventId);
        if (data != null)
        {
            _context.EventTables.Remove(data);
            await _context.SaveChangesAsync();
        }
        return Ok(IdentityResult.Success);
    }

    [HttpGet("feedback/list")]
    public async Task<IActionResult> GetLeadFeedbackAsync([FromQuery] UserFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = from a in _context.LeadFeedbacks
                    join b in _context.Leads on a.LeadId equals b.Id
                    join c in _context.Tables on a.TableId equals c.Id into ac
                    from c in ac.DefaultIfEmpty()
                    join j in _context.JobKinds on a.JobKindId equals j.Id into jb
                    from j in jb.DefaultIfEmpty()
                    join d in _context.Users on b.CreatedBy equals d.Id
                    select new
                    {
                        a.InterestLevel,
                        JobTitle = j.Name,
                        a.FinancialSituation,
                        a.CheckinTime,
                        a.CheckoutTime,
                        a.RejectReason,
                        b.SourceId,
                        ToName = d.Name,
                        b.AttendanceId,
                        TableName = c.Name,
                        b.Name,
                        b.Email,
                        b.PhoneNumber,
                        b.Status,
                        b.EventId,
                        b.EventDate,
                        b.SalesId,
                        d.SmId,
                        d.TmId,
                        d.DosId,
                        d.DotId
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (_hcaService.IsUserInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.SmId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Dos))
        {
            query = query.Where(x => x.DosId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Dot))
        {
            query = query.Where(x => x.DotId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
        {
            query = query.Where(x => x.TmId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Sales))
        {
            query = query.Where(x => x.SalesId == userId);
        }
        query = query.OrderByDescending(x => x.EventDate);
        return Ok(await ListResult<object>.Success(query, filterOptions));
    }

    [HttpPost("table/remove/{id}")]
    public async Task<IActionResult> TableRemoveAsync([FromRoute] Guid id)
    {
        var table = await _context.EventTables.FindAsync(id);
        if (table == null) return BadRequest("table not found");
        _context.EventTables.Remove(table);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("list-lead-history/{id}")]
    public async Task<IActionResult> ListLeadHistoryAsync([FromQuery] FilterOptions filterOptions, [FromRoute] Guid id)
    {
        var query = from a in _context.LeadHistories
                    join b in _context.Leads on a.LeadId equals b.Id
                    join c in _context.Users on a.CreatedBy equals c.Id
                    join d in _context.Users on a.ToById equals d.Id into ad
                    from d in ad.DefaultIfEmpty()
                    join e in _context.Tables on a.TableId equals e.Id into ae
                    from e in ae.DefaultIfEmpty()
                    join f in _context.Attendances on a.AttendanceId equals f.Id into af
                    from f in af.DefaultIfEmpty()
                    where a.LeadId == id
                    select new
                    {
                        a.Id,
                        a.LeadId,
                        a.EventDate,
                        a.Note,
                        a.TelesaleId,
                        a.SalesId,
                        SalesName = _context.Users.First(x => x.Id == a.SalesId).Name,
                        b.PhoneNumber,
                        b.Name,
                        a.AttendanceId,
                        CreatorName = c.Name,
                        ToName = d.Name,
                        a.TableId,
                        b.IdentityNumber,
                        a.EventId,
                        a.CheckinTime,
                        a.CheckoutTime,
                        TableName = e.Name,
                        AttendanceName = f.Name,
                        b.Gender,
                        b.DateOfBirth,
                        a.TransportId
                    };
        query = query.OrderByDescending(x => x.EventDate);
        return Ok(await ListResult<object>.Success(query, filterOptions));
    }

    [HttpPost("export-lead")]
    public async Task<IActionResult> ExportLeadAsync([FromBody] ExportDateFilterOptions filterOptions)
    {
        try
        {
            var attendances = await _context.Attendances.AsNoTracking().ToListAsync();
            var user = await _userManager.FindByIdAsync(User.GetId().ToString());
            if (user is null) return Unauthorized();
            var query = from a in _context.Leads
                        join e in _context.Events on a.EventId equals e.Id
                        join b in _context.Users on a.TelesaleId equals b.Id into ab
                        from b in ab.DefaultIfEmpty()
                        join c in _context.LeadFeedbacks on a.Id equals c.LeadId into ac
                        from c in ac.DefaultIfEmpty()
                        join d in _context.Users on a.SalesId equals d.Id into ad
                        from d in ad.DefaultIfEmpty()
                        join f in _context.Sources on a.SourceId equals f.Id into cf
                        from f in cf.DefaultIfEmpty()
                        join contract in _context.Contracts on a.Id equals contract.LeadId into contractJoin
                        from contract in contractJoin.DefaultIfEmpty()
                        where a.BranchId == user.BranchId && a.Status != LeadStatus.Pending && a.Status != LeadStatus.Approved
                        select new
                        {
                            a.Id,
                            a.Name,
                            a.PhoneNumber,
                            a.EventDate,
                            a.EventId,
                            TeleName = b.Name,
                            a.SourceId,
                            ContractCode = contract.Code,
                            c.CheckinTime,
                            c.CheckoutTime,
                            a.Status,
                            a.AttendanceId,
                            SalesName = d.Name,
                            a.Note,
                            EventName = e.Name,
                            SourceName = f.Name
                        };
            if (filterOptions.FromDate != null)
            {
                query = query.Where(x => x.EventDate >= filterOptions.FromDate);
            }
            if (filterOptions.ToDate != null)
            {
                query = query.Where(x => x.EventDate <= filterOptions.ToDate);
            }

            var data = await query.ToListAsync();

            using var pgk = new ExcelPackage();
            var ws = pgk.Workbook.Worksheets.Add("Sheet1");

            ws.Cells[1, 1].Value = "STT";
            ws.Cells[1, 2].Value = "Họ và tên";
            ws.Cells[1, 3].Value = "SDT";
            ws.Cells[1, 4].Value = "Trạng thái bàn";
            ws.Cells[1, 5].Value = "Ngày tham gia";
            ws.Cells[1, 6].Value = "Giờ tham gia";
            ws.Cells[1, 7].Value = "Telesales";
            ws.Cells[1, 8].Value = "Nguồn";
            ws.Cells[1, 9].Value = "Số hợp đồng";
            ws.Cells[1, 10].Value = "Giờ check-in";
            ws.Cells[1, 11].Value = "Giờ check-out";
            ws.Cells[1, 12].Value = "Trạng thái";
            ws.Cells[1, 13].Value = "Sales";
            ws.Cells[1, 14].Value = "Ghi chú";

            var row = 2;
            foreach (var item in data)
            {
                var status = "Chờ duyệt";
                switch (item.Status)
                {
                    case LeadStatus.Pending:
                        break;
                    case LeadStatus.Approved:
                        status = "Đã duyệt";
                        break;
                    case LeadStatus.Checkin:
                        status = "Check-in";
                        break;
                    case LeadStatus.LeadAccept:
                        status = "Chốt deal";
                        break;
                    case LeadStatus.LeadReject:
                        status = "Từ chối";
                        break;
                    case LeadStatus.ReInvite:
                        status = "Mời lại";
                        break;
                    default:
                        break;
                }
                ws.Cells[row, 1].Value = row - 1;
                ws.Cells[row, 2].Value = item.Name;
                ws.Cells[row, 3].Value = item.PhoneNumber;
                var attendance = attendances.FirstOrDefault(x => x.Id == item.AttendanceId);
                ws.Cells[row, 4].Value = attendance?.Name;
                ws.Cells[row, 5].Value = item.EventDate.ToString("dd-MM-yyyy");
                ws.Cells[row, 6].Value = item.EventName;
                ws.Cells[row, 7].Value = item.TeleName;
                ws.Cells[row, 8].Value = item.SourceName;
                ws.Cells[row, 9].Value = item.ContractCode;
                ws.Cells[row, 10].Value = item.CheckinTime?.ToString();
                ws.Cells[row, 11].Value = item.CheckoutTime?.ToString();
                ws.Cells[row, 12].Value = status;
                ws.Cells[row, 13].Value = item.SalesName;
                ws.Cells[row, 14].Value = item.Note;

                row++;
            }
            ws.Row(1).Style.Font.Bold = true;
            var dataRange = ws.Cells[1, 1, row, 15];
            // Apply borders to the entire data range
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            for (int i = 1; i < 15; i++)
            {
                ws.Column(i).AutoFit();
            }

            await pgk.SaveAsync();

            var fileName = $"data-keyin-nuras";

            return File(await pgk.GetAsByteArrayAsync(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("lead/status")]
    public async Task<IActionResult> ChangeStatusLeadAsync([FromBody] Lead args)
    {
        var lead = await _context.Leads.FindAsync(args.Id);
        if (lead is null) return BadRequest();
        lead.Status = args.Status;
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("change-status")]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeStatusArgs args)
    {
        if (!User.IsInRole(RoleName.Admin)) return BadRequest("Không thể thay đổi trạng thái của Key-In");
        var lead = await _context.Leads.FindAsync(args.LeadId);
        if (lead is null) return BadRequest("Không tìm thấy Key-In");
        if (lead.Status == args.Status) return BadRequest("Trạng thái không thay đổi!");
        lead.Status = args.Status;
        _context.Leads.Update(lead);
        await _appLogService.AddAsync($"Thay đổi trạng thái Key-In: {lead.Name} từ {lead.Status} sang {args.Status}");
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("status-options")]
    public IActionResult GetStatusOptions()
    {
        var status = Enum.GetValues(typeof(LeadStatus)).Cast<LeadStatus>().Select(x => new
        {
            label = EnumHelper.GetDisplayName(x),
            value = x
        }).ToList();
        return Ok(status);
    }

    [HttpGet("blacklist")]
    public async Task<IActionResult> GetBlacklistAsync([FromQuery] BlacklistFilterOptions filterOptions) => Ok(await _contactService.GetBlacklistAsync(filterOptions));

    [HttpPost("block")]
    public async Task<IActionResult> BlockAsync([FromBody] BlockContactArgs args) => Ok(await _contactService.BlockAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ContactUpdateArgs args) => Ok(await _contactService.UpdateAsync(args));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ContactCreateArgs args) => Ok(await _contactService.CreateAsync(args));

    [HttpPost("book")]
    public async Task<IActionResult> BookAsync([FromBody] ContactBookArgs args) => Ok(await _contactService.BookAsync(args));

    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync([FromForm] ContactImportArgs args) => Ok(await _contactService.ImportAsync(args));

    [HttpGet("unassigned-list")]
    public async Task<IActionResult> GetUnassignedListAsync([FromQuery] UnassignedFilterOptions filterOptions) => Ok(await _contactService.GetUnassignedListAsync(filterOptions));

    [HttpPost("assign-source")]
    public async Task<IActionResult> AssignSourceAsync([FromBody] ContactAssignSourceArgs args) => Ok(await _contactService.AssignSourceAsync(args));

    [HttpPost("confirm1/{id}")]
    public async Task<IActionResult> Confirm1Async([FromRoute] Guid id) => Ok(await _contactService.Confirm1Async(id));

    [HttpPost("confirm2/{id}")]
    public async Task<IActionResult> Confirm2Async([FromRoute] Guid id) => Ok(await _contactService.Confirm2Async(id));

    [HttpGet("tmr-report")]
    public async Task<IActionResult> GetTmrReportAsync() => Ok(await _contactService.GetTmrReportAsync());
}
