using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contacts.Args;
using Waffle.Core.Services.Contacts.Filters;
using Waffle.Core.Services.Contacts.Models;
using Waffle.Core.Services.Contacts.Results;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;
using Waffle.Models.Filters;

namespace Waffle.Infrastructure.Repositories;

public class ContactRepository(ApplicationDbContext context, IHCAService _hcaService) : EfRepository<Contact>(context), IContactRepository
{
    public async Task<ListResult<object>> DialedCallsAsync(ContactFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = from c in _context.Contacts
                    join u in _context.Users on c.UserId equals u.Id
                    join s in _context.Sources on c.SourceId equals s.Id
                    join t in _context.Teams on u.TeamId equals t.Id into ut
                    from t in ut.DefaultIfEmpty()
                    where c.Status != ContactStatus.Blacklisted
                    where c.LastCallTime != null
                    select new
                    {
                        c.Id,
                        c.Name,
                        c.PhoneNumber,
                        c.CreatedDate,
                        c.UserId,
                        TeleName = u.Name,
                        CalledAt = c.LastCallTime,
                        Note = _context.CallHistories
                            .Where(ch => ch.ContactId == c.Id)
                            .OrderByDescending(ch => ch.CreatedDate)
                            .Select(ch => ch.Note)
                            .FirstOrDefault(),
                        SourceName = s.Name,
                        CallStatusId = _context.CallHistories.Where(ch => ch.ContactId == c.Id).OrderByDescending(ch => ch.CreatedDate).Select(ch => ch.CallStatusId).FirstOrDefault(),
                        Age = _context.CallHistories
                            .Where(ch => ch.ContactId == c.Id)
                            .OrderByDescending(ch => ch.CreatedDate)
                            .Select(ch => ch.Age)
                            .FirstOrDefault(),
                        FollowUpDate = c.FollowUpdate,
                        Job = _context.CallHistories
                            .Where(ch => ch.ContactId == c.Id)
                            .OrderByDescending(ch => ch.CreatedDate)
                            .Select(ch => ch.Job)
                            .FirstOrDefault(),
                        c.ExtraStatus,
                        IsBooked = _context.Leads.Any(x => x.PhoneNumber == c.PhoneNumber),
                        c.SourceId,
                        s.TeamId,
                        TeamName = t.Name
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(c => c.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.CallStatusId.HasValue)
        {
            query = query.Where(c => c.CallStatusId == filterOptions.CallStatusId);
        }
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(c => c.CalledAt != null && c.CalledAt >= filterOptions.FromDate.Value.Date && c.CalledAt <= filterOptions.ToDate.Value.Date);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(c => c.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Job))
        {
            query = query.Where(c => c.Job != null && c.Job.ToLower().Contains(filterOptions.Job.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Age))
        {
            query = query.Where(c => c.Age != null && c.Age.ToLower().Contains(filterOptions.Age.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.ExtraStatus))
        {
            query = query.Where(c => c.ExtraStatus != null && c.ExtraStatus.ToLower().Contains(filterOptions.ExtraStatus.ToLower()));
        }
        if (filterOptions.SourceId.HasValue)
        {
            query = query.Where(x => x.SourceId == filterOptions.SourceId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Note))
        {
            query = query.Where(x => x.Note != null && x.Note.ToLower().Contains(filterOptions.Note.ToLower()));
        }
        if (filterOptions.IsBooked.HasValue)
        {
            query = query.Where(c => c.IsBooked == filterOptions.IsBooked);
        }
        if (_hcaService.IsUserInRole(RoleName.Telesales))
        {
            query = query.Where(c => c.UserId == userId);
        }
        query = query.OrderByDescending(c => c.CalledAt);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<ListResult<object>> GetBlacklistAsync(BlacklistFilterOptions filterOptions)
    {
        var query = from c in _context.Contacts
                    where c.Status == ContactStatus.Blacklisted
                    select new
                    {
                        c.Id,
                        c.Name,
                        c.Email,
                        c.PhoneNumber,
                        c.Note,
                        c.Address,
                        c.CreatedDate,
                        c.Status
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(c => c.Name.Contains(filterOptions.Name, StringComparison.CurrentCultureIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(c => c.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        query = query.OrderByDescending(c => c.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<TResult> GetReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions)
    {
        var sourceGroups = await _context.Teams.Where(x => filterOptions.TeamId == null || x.Id == filterOptions.TeamId)
            .Select(x => new { x.Id, x.Name })
            .ToListAsync();

        var callStatuses = await _context.CallStatuses.ToListAsync();
        var contacts = await _context.Contacts.ToListAsync();
        var sources = await _context.Sources.Include(s => s.TypeOfData).ToListAsync();
        var showUps = await _context.Leads.Where(l => l.Confirm2 == Confirm2.CONFIRM).Select(x => new
        {
            x.Id,
            x.Duplicated,
            x.Status,
            x.PhoneNumber
        }).ToListAsync();
        var callHistories = await (from ch in _context.CallHistories
                                   join cs in _context.CallStatuses on ch.CallStatusId equals cs.Id
                                   where filterOptions.FromDate == null || ch.CreatedDate >= filterOptions.FromDate.Value.Date
                                   where filterOptions.ToDate == null || ch.CreatedDate <= filterOptions.ToDate.Value.Date
                                   select new
                                   {
                                       ch.ContactId,
                                       ch.CallStatusId,
                                       cs.Code,
                                       cs.Type
                                   }).ToListAsync();

        // Calculate totals
        var totalContacts = contacts.Count;
        var totalContactsWithType = contacts.Count(c => sources.Any(s => s.Id == c.SourceId && s.TypeOfDataId != null));
        var totalContactsCalled = callHistories.Select(ch => ch.ContactId).Distinct().Count();
        var totalNotUpdated = contacts.Count(c => !callHistories.Any(ch => ch.ContactId == c.Id));
        var totalCf1 = contacts.Count(c => c.Confirm1 == true);
        var totalShowUp = showUps.Count(l => !l.Duplicated);
        var totalDeal = showUps.Count(l => !l.Duplicated && l.Status == LeadStatus.CloseDeal);

        var result = new
        {
            Total = new
            {
                SourceGroup = "Total",
                ContactImport = totalContacts,
                ContactStartCase = totalContactsWithType,
                Total = totalContacts,
                TeleNotUpdate = totalNotUpdated,
                CF1 = totalCf1,
                Showup = totalShowUp,
                Deal = totalDeal,
                PercentCFTotalContacted = totalContactsCalled > 0 ? (double)totalCf1 / totalContactsCalled * 100 : 0,
                PercentShowupCF = totalCf1 > 0 ? (double)totalShowUp / totalCf1 * 100 : 0,
                PercentDealShowup = totalShowUp > 0 ? (double)totalDeal / totalShowUp * 100 : 0
            },
            Teams = sourceGroups.OrderBy(t => t.Name).Select(team =>
            {
                var teamSources = sources.Where(s => s.TeamId == team.Id).ToList();
                var teamContacts = contacts.Where(c => teamSources.Any(s => s.Id == c.SourceId)).ToList();
                var teamContactIds = teamContacts.Select(c => c.Id).ToList();
                var teamCallHistories = callHistories.Where(ch => teamContactIds.Contains(ch.ContactId)).ToList();
                var teamCf1 = teamContacts.Where(c => c.Confirm1 == true).Count();
                var teamShowUp = showUps.Where(l => !l.Duplicated && teamContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
                var teamDeal = showUps.Where(l => !l.Duplicated && l.Status == LeadStatus.CloseDeal && teamContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
                var teamContactsCalled = teamCallHistories.Select(ch => ch.ContactId).Distinct().Count();

                return new
                {
                    SourceGroup = team.Name,
                    SourceName = "Total Source Name",
                    ContactImport = teamContacts.Count,
                    Total = teamContacts.Count,
                    CF1 = teamCf1,
                    Showup = teamShowUp,
                    Deal = teamDeal,
                    PercentCFTotalContacted = teamContactsCalled > 0 ? (double)teamCf1 / teamContactsCalled * 100 : 0,
                    PercentShowupCF = teamCf1 > 0 ? (double)teamShowUp / teamCf1 * 100 : 0,
                    PercentDealShowup = teamShowUp > 0 ? (double)teamDeal / teamShowUp * 100 : 0,
                    Sources = teamSources.OrderBy(s => s.Name).Select(source =>
                    {
                        var sourceContacts = teamContacts.Where(c => c.SourceId == source.Id).ToList();
                        var sourceContactIds = sourceContacts.Select(c => c.Id).ToList();
                        var sourceCf1 = sourceContacts.Where(c => c.Confirm1 == true).Count();
                        var sourceShowUp = showUps.Where(l => !l.Duplicated && sourceContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
                        var sourceDeal = showUps.Where(l => !l.Duplicated && l.Status == LeadStatus.CloseDeal && sourceContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
                        var sourceCallHistories = callHistories.Where(ch => sourceContactIds.Contains(ch.ContactId)).ToList();
                        var sourceContactsCalled = sourceCallHistories.Select(ch => ch.ContactId).Distinct().Count();

                        return new
                        {
                            SourceName = source.Name,
                            ContactImport = sourceContacts.Count,
                            Total = sourceContacts.Count,
                            CF1 = sourceCf1,
                            Showup = sourceShowUp,
                            Deal = sourceDeal,
                            PercentCFTotalContacted = sourceContactsCalled > 0 ? (double)sourceCf1 / sourceContactsCalled * 100 : 0,
                            PercentShowupCF = sourceCf1 > 0 ? (double)sourceShowUp / sourceCf1 * 100 : 0,
                            PercentDealShowup = sourceShowUp > 0 ? (double)sourceDeal / sourceShowUp * 100 : 0
                        };
                    }).ToList()
                };
            }).ToList()
        };

        return TResult.Ok(result);
    }

    public async Task<TResult<object>> GetTmrReportAsync()
    {
        var totalAvailableAssign = await _context.Contacts.Where(c => c.Status != ContactStatus.Blacklisted && c.UserId != null).CountAsync();
        var totalCalled = await _context.CallHistories.Select(c => c.ContactId).Distinct().CountAsync();
        var totalContact = await _context.Contacts.Where(c => c.Status != ContactStatus.Blacklisted).CountAsync();
        var result = new
        {
            totalAvailableAssign,
            totalCalled,
            totalContact,
            TotalNotContacted = totalContact - totalCalled
        };
        return TResult<object>.Ok(result);
    }

    public async Task<List<Contact>> GetUnassignedContactsAsync(int numberOfContact, int sourceId)
    {
        var query = _context.Contacts
            .Where(c => c.UserId == null && c.Status != ContactStatus.Blacklisted)
            .Where(x => x.SourceId == sourceId)
            .OrderBy(c => Guid.NewGuid())
            .Take(numberOfContact);
        return await query.ToListAsync();
    }

    public async Task<ListResult<object>> GetUnassignedListAsync(UnassignedFilterOptions filterOptions)
    {
        var query = from a in _context.Contacts
                    join c in _context.Users on a.CreatedBy equals c.Id
                    where a.UserId == null
                    select new
                    {
                        a.Id,
                        a.PhoneNumber,
                        a.Email,
                        a.CreatedDate,
                        a.Gender,
                        a.CreatedBy,
                        a.Address,
                        a.Note,
                        a.Name,
                        CreatorName = c.Name,
                        a.SourceId
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (filterOptions.SourceId.HasValue)
        {
            query = query.Where(x => x.SourceId == filterOptions.SourceId);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public Task<bool> IsPhoneExistAsync(string phoneNumber) => _context.Contacts.AnyAsync(x => x.PhoneNumber == phoneNumber);

    public async Task<ListResult<dynamic>> ListAsync(ContactFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = from a in _context.Contacts
                    join b in _context.Users on a.UserId equals b.Id into ab
                    from b in ab.DefaultIfEmpty()
                    join s in _context.Sources on a.SourceId equals s.Id into asource
                    from s in asource.DefaultIfEmpty()
                    join t in _context.Teams on b.TeamId equals t.Id into bteam
                    from t in bteam.DefaultIfEmpty()
                    join tod in _context.TypeOfDatas on s.TypeOfDataId equals tod.Id into stype
                    from tod in stype.DefaultIfEmpty()
                    where a.LastCallTime == null
                    select new ContactListItem
                    {
                        Id = a.Id,
                        PhoneNumber = a.PhoneNumber,
                        CreatedDate = a.CreatedDate,
                        Name = a.Name,
                        Note = a.Note,
                        TelesalesId = a.UserId,
                        TelesalesName = b.Name,
                        ShowUp = _context.Leads.Any(x => x.PhoneNumber == a.PhoneNumber && !x.Duplicated && (x.Status == LeadStatus.Checkin || x.Status == LeadStatus.CloseDeal)),
                        SourceId = a.SourceId,
                        TmId = b.TmId,
                        DotId = b.DotId,
                        DosId = b.DosId,
                        Confirm1 = a.Confirm1,
                        SourceName = s.Name,
                        TypeOfDataId = s.TypeOfDataId,
                        TypeOfDataName = tod.Name,
                        SourceType = tod.Source,
                        Name2 = a.Name2,
                        PhoneNumber2 = a.PhoneNumber2
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToLower().Contains(filterOptions.PhoneNumber.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.SourceId.HasValue)
        {
            query = query.Where(x => x.SourceId == filterOptions.SourceId);
        }
        if (filterOptions.IsBooked.HasValue)
        {
            query = query.Where(x => x.ShowUp == filterOptions.IsBooked);
        }
        if (filterOptions.Confirm1.HasValue)
        {
            query = query.Where(x => x.Confirm1 == filterOptions.Confirm1);
        }
        if (filterOptions.TypeOfDataId.HasValue)
        {
            query = query.Where(x => x.TypeOfDataId == filterOptions.TypeOfDataId);
        }
        if (filterOptions.SourceType.HasValue)
        {
            query = query.Where(x => x.SourceType == filterOptions.SourceType);
        }
        if (filterOptions.TelesalesId.HasValue)
        {
            query = query.Where(x => x.TelesalesId == filterOptions.TelesalesId);
        }
        if (_hcaService.IsUserInRole(RoleName.Telesales))
        {
            query = query.Where(x => x.TelesalesId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
        {
            query = query.Where(x => x.TmId == userId);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<dynamic>.Success(query, filterOptions);
    }

    public async Task<ListResult<object>> GetAttendanceScheduleListAsync(ContactFilterOptions filterOptions)
    {
        var query = from b in _context.Users
                    join c in _context.Leads on b.Id equals c.CreatedBy
                    join d in _context.Events on c.EventId equals d.Id
                    join ur in _context.UserRoles on b.Id equals ur.UserId
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where r.Name == RoleName.Telesales || r.Name == RoleName.TelesaleManager || r.Name == RoleName.Dot
                    select new
                    {
                        c.Id,
                        c.PhoneNumber,
                        c.Email,
                        c.CreatedDate,
                        c.Name,
                        c.Note,
                        StaffId = c.CreatedBy,
                        StaffName = b.Name,
                        StaffAvatar = b.Avatar,
                        c.Confirm2,
                        c.EventDate,
                        EventName = d.Name,
                        b.TmId,
                        c.EventId,
                        ContactNote = _context.Contacts.Where(x => x.PhoneNumber == c.PhoneNumber).Select(x => x.Note).FirstOrDefault()
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.Contains(filterOptions.Name));
        }
        if (filterOptions.Confirm2.HasValue)
        {
            query = query.Where(x => x.Confirm2 == filterOptions.Confirm2);
        }
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(x => x.EventDate.Date >= filterOptions.FromDate.Value.Date && x.EventDate.Date <= filterOptions.ToDate.Value.Date);
        }
        if (filterOptions.EventId.HasValue)
        {
            query = query.Where(x => x.EventId == filterOptions.EventId);
        }
        if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
        {
            query = query.Where(x => x.TmId == _hcaService.GetUserId());
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public void Update(Contact contact) => _context.Contacts.Update(contact);

    public async Task<IEnumerable<string?>> AllPhoneNumbersAsync() => await _context.Contacts.Select(c => c.PhoneNumber).Distinct().ToListAsync();

    public async Task<TResult<byte[]?>> ExportReportDataSourceAsync(ReportDataSourceFilterOptions filterOptions)
    {
        var sourceGroups = await _context.Teams.Where(x => filterOptions.TeamId == null || x.Id == filterOptions.TeamId)
            .Select(x => new { x.Id, x.Name })
            .ToListAsync();

        var callStatuses = await _context.CallStatuses.ToListAsync();
        var contacts = await _context.Contacts.ToListAsync();
        var sources = await _context.Sources.Include(s => s.TypeOfData).ToListAsync();
        var showUps = await _context.Leads.Where(x => x.Confirm2 == Confirm2.CONFIRM).Select(x => new
        {
            x.Id,
            x.Duplicated,
            x.Status,
            x.PhoneNumber
        }).ToListAsync();
        var callHistories = await (from ch in _context.CallHistories
                                   join cs in _context.CallStatuses on ch.CallStatusId equals cs.Id
                                   where filterOptions.FromDate == null || ch.CreatedDate >= filterOptions.FromDate.Value.Date
                                   where filterOptions.ToDate == null || ch.CreatedDate <= filterOptions.ToDate.Value.Date
                                   select new
                                   {
                                       ch.ContactId,
                                       ch.CallStatusId,
                                       cs.Code,
                                       cs.Type
                                   }).ToListAsync();

        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Sheet1");

        // Header setup
        ws.Cells[1, 1].Value = "Source Group";
        ws.Cells[1, 1, 2, 1].Merge = true;
        ws.Cells[1, 2].Value = "Source Name";
        ws.Cells[1, 2, 2, 2].Merge = true;
        ws.Cells[1, 3].Value = "Type of Data";
        ws.Cells[1, 3, 1, 5].Merge = true;
        ws.Cells[2, 3].Value = "Contact Import";
        ws.Cells[2, 4].Value = "Contact Start Case";
        ws.Cells[2, 5].Value = "Total";
        ws.Cells[1, 6].Value = "Tổng số chưa liên hệ";
        ws.Cells[1, 6, 1, 8].Merge = true;
        ws.Cells[2, 6].Value = "1. Không nghe máy";
        ws.Cells[2, 7].Value = "2. Thuê bao";
        ws.Cells[2, 8].Value = "Tổng (1)";
        ws.Cells[1, 9].Value = "Tổng số đã liên hệ";
        ws.Cells[1, 9, 1, 13].Merge = true;
        ws.Cells[2, 9].Value = "7. Không đạt y/c";
        ws.Cells[2, 10].Value = "6. Đạt y/c";
        ws.Cells[2, 11].Value = "5. Gọi lại sau";
        ws.Cells[2, 12].Value = "4. Ngoại tỉnh";
        ws.Cells[2, 13].Value = "Tổng (2)";
        ws.Cells[1, 14].Value = "Total Invite";
        ws.Cells[1, 14, 1, 15].Merge = true;
        ws.Cells[2, 14].Value = "CF1";
        ws.Cells[2, 15].Value = "Khách cân nhắc";
        ws.Cells[1, 16].Value = "%CF/Total Contacted";
        ws.Cells[1, 16, 2, 16].Merge = true;
        ws.Cells[1, 17].Value = "Showup";
        ws.Cells[1, 17, 2, 17].Merge = true;
        ws.Cells[1, 18].Value = "%Showup/CF";
        ws.Cells[1, 18, 2, 18].Merge = true;
        ws.Cells[1, 19].Value = "Deal";
        ws.Cells[1, 19, 2, 19].Merge = true;
        ws.Cells[1, 20].Value = "%Deal/Showup";
        ws.Cells[1, 20, 2, 20].Merge = true;

        using (var headerRange = ws.Cells[1, 1, 2, 20])
        {
            headerRange.Style.Font.Bold = true;
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        }

        int currentRow = 3;

        // Total row
        var totalContacts = contacts.Count;
        var totalContactsWithType = contacts.Where(c => sources.Any(s => s.Id == c.SourceId && s.TypeOfDataId != null)).Count();
        var totalContactsCalled = callHistories.Select(ch => ch.ContactId).Distinct().Count();
        var totalNoPickup = contacts.Where(c => callHistories.Any(ch => ch.Type == CallStatusType.NO_PICK_UP)).Count();
        var totalCf1 = contacts.Count(c => c.Confirm1 == true);
        var totalShowUp = showUps.Count(l => !l.Duplicated);
        var totalDeal = showUps.Count(l => !l.Duplicated && l.Status == LeadStatus.CloseDeal);
        var totalConsider = contacts.Count(x => callHistories.Any(ch => ch.ContactId == x.Id && ch.Code == CallStatusCode.CONSIDER));
        var totalCallLater = contacts.Count(c => callHistories.Any(ch => ch.ContactId == c.Id && ch.Code == CallStatusCode.CALL_LATER));
        var totalLocation = contacts.Count(c => callHistories.Any(ch => ch.ContactId == c.Id && ch.Type == CallStatusType.LOCATION));

        ws.Cells[currentRow, 1].Value = "Total";
        ws.Cells[currentRow, 3].Value = totalContacts;
        ws.Cells[currentRow, 4].Value = totalContactsWithType;
        ws.Cells[currentRow, 5].Value = totalContacts;
        ws.Cells[currentRow, 6].Value = totalNoPickup;
        ws.Cells[currentRow, 11].Value = totalCallLater;
        ws.Cells[currentRow, 12].Value = totalLocation;
        ws.Cells[currentRow, 14].Value = totalCf1;
        ws.Cells[currentRow, 15].Value = totalConsider;
        ws.Cells[currentRow, 17].Value = totalShowUp;
        ws.Cells[currentRow, 19].Value = totalDeal;
        currentRow++;

        // Data by team and source
        foreach (var team in sourceGroups.OrderBy(t => t.Name))
        {
            var teamSources = sources.Where(s => s.TeamId == team.Id).ToList();
            var teamContacts = contacts.Where(c => teamSources.Any(s => s.Id == c.SourceId)).ToList();
            var teamContactIds = teamContacts.Select(c => c.Id).ToList();
            var teamCallHistories = callHistories.Where(ch => teamContactIds.Contains(ch.ContactId)).ToList();
            var teamCf1 = teamContacts.Where(c => c.Confirm1 == true).Count();
            var teamShowUp = showUps.Where(l => !l.Duplicated && teamContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
            var teamDeal = showUps.Where(l => !l.Duplicated && l.Status == LeadStatus.CloseDeal && teamContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
            var teamLocation = teamCallHistories.Count(ch => ch.Type == CallStatusType.LOCATION);
            var teamConsider = teamCallHistories.Count(ch => ch.Code == CallStatusCode.CONSIDER);
            var teamCallLater = teamCallHistories.Count(ch => ch.Code == CallStatusCode.CALL_LATER);

            ws.Cells[currentRow, 1].Value = team.Name;
            ws.Cells[currentRow, 2].Value = "Total Source Name";
            ws.Cells[currentRow, 3].Value = teamContacts.Count;
            ws.Cells[currentRow, 5].Value = teamContacts.Count;
            ws.Cells[currentRow, 11].Value = teamCallLater;
            ws.Cells[currentRow, 12].Value = teamLocation;
            ws.Cells[currentRow, 14].Value = teamCf1;
            ws.Cells[currentRow, 15].Value = teamConsider;
            ws.Cells[currentRow, 17].Value = teamShowUp;
            ws.Cells[currentRow, 19].Value = teamDeal;
            currentRow++;

            foreach (var source in teamSources.OrderBy(s => s.Name))
            {
                var sourceContacts = teamContacts.Where(c => c.SourceId == source.Id).ToList();
                var sourceContactIds = sourceContacts.Select(c => c.Id).ToList();
                var sourceCf1 = sourceContacts.Where(c => c.Confirm1 == true).Count();
                var sourceShowUp = showUps.Where(l => !l.Duplicated && sourceContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();
                var sourceDeal = showUps.Where(l => !l.Duplicated && l.Status == LeadStatus.CloseDeal && sourceContacts.Any(c => c.PhoneNumber == l.PhoneNumber)).Count();

                ws.Cells[currentRow, 2].Value = source.Name;
                ws.Cells[currentRow, 3].Value = sourceContacts.Count;
                ws.Cells[currentRow, 5].Value = sourceContacts.Count;
                ws.Cells[currentRow, 14].Value = sourceCf1;
                ws.Cells[currentRow, 17].Value = sourceShowUp;
                ws.Cells[currentRow, 19].Value = sourceDeal;
                currentRow++;
            }
        }

        ws.Cells[ws.Dimension.Address].AutoFitColumns();
        using (var dataRange = ws.Cells[1, 1, currentRow - 1, 20])
        {
            dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        return TResult<byte[]?>.Ok(await package.GetAsByteArrayAsync());
    }

    public async Task<TResult<byte[]?>> ExportTmrDataReportAsync(TmrDataReportFilterOptions filterOptions)
    {
        var callStatuses = await _context.CallStatuses.ToListAsync();

        int? GetStatusId(string name) =>
            callStatuses.FirstOrDefault(x => x.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))?.Id;

        var teleNotUpdateId = GetStatusId("Tele not update");
        var tempLockedId = GetStatusId("Temporary locked/Wrong number/Knm");
        var notEnoughId = GetStatusId("Not Enough Qualify");
        var meetRequireId = GetStatusId("Meet Require");
        var refuseId = GetStatusId("Refuse to talk");
        var locationId = GetStatusId("Location");

        var data = await (from t in _context.Teams
                          join u in _context.Users on t.Id equals u.TeamId into tu
                          from u in tu.DefaultIfEmpty()
                          join c in _context.Contacts on u.Id equals c.UserId into uc
                          from c in uc.DefaultIfEmpty()
                          where filterOptions.TeamId == null || t.Id == filterOptions.TeamId
                          select new
                          {
                              TeamId = t.Id,
                              TeamName = t.Name,
                              UserId = (Guid?)u.Id,
                              UserName = u.UserName,
                              FullName = u.Name,
                              ContactId = (Guid?)c.Id,
                              ContactStatus = (ContactStatus?)c.Status,
                              CallStatusId = c.CallStatusId
                          }).ToListAsync();

        (int tele, int temp, int notEnough, int meet, int refuse, int location) GetCounts(IEnumerable<dynamic> items) =>
            (
                tele: items.Count(x => x.ContactId != null && teleNotUpdateId.HasValue && x.CallStatusId == teleNotUpdateId),
                temp: items.Count(x => x.ContactId != null && tempLockedId.HasValue && x.CallStatusId == tempLockedId),
                notEnough: items.Count(x => x.ContactId != null && notEnoughId.HasValue && x.CallStatusId == notEnoughId),
                meet: items.Count(x => x.ContactId != null && meetRequireId.HasValue && x.CallStatusId == meetRequireId),
                refuse: items.Count(x => x.ContactId != null && refuseId.HasValue && x.CallStatusId == refuseId),
                location: items.Count(x => x.ContactId != null && locationId.HasValue && x.CallStatusId == locationId)
            );

        var teams = data.GroupBy(x => new { x.TeamId, x.TeamName })
            .Select(g => new
            {
                TeamId = g.Key.TeamId,
                LeaderName = g.Key.TeamName,
                TotalAssign = g.Count(x => x.ContactId != null),
                TotalAvailableContact = g.Count(x => x.ContactId != null && x.ContactStatus != ContactStatus.Blacklisted),
                CallStatusCounts = GetCounts(g),
                Users = g.Where(x => x.UserId != null)
                    .GroupBy(x => new { x.UserId, x.UserName, x.FullName })
                    .Select(ug => new
                    {
                        UserId = ug.Key.UserId,
                        UserName = ug.Key.UserName,
                        FullName = ug.Key.FullName,
                        TotalAssign = ug.Count(x => x.ContactId != null),
                        TotalAvailableContact = ug.Count(x => x.ContactId != null && x.ContactStatus != ContactStatus.Blacklisted),
                        CallStatusCounts = GetCounts(ug)
                    }).ToList()
            }).ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            worksheet.Cells[1, 1].Value = "Team Name";
            worksheet.Cells[1, 2].Value = "User Name";
            worksheet.Cells[1, 3].Value = "Full Name";
            worksheet.Cells[1, 4].Value = "Assign";

            worksheet.Cells[2, 4].Value = "Total Assign";
            worksheet.Cells[2, 5].Value = "Total Available Contact";

            if (filterOptions.ViewType == TmrDataReportViewType.CallStatus)
            {
                worksheet.Cells[1, 4].Value = "Call Status";
                worksheet.Cells[2, 4].Value = "Tele not update";
                worksheet.Cells[2, 5].Value = "Temporary locked/Wrong number/Knm";
                worksheet.Cells[2, 6].Value = "Not Enough Qualify";
                worksheet.Cells[2, 7].Value = "Meet Require";
                worksheet.Cells[2, 8].Value = "Refuse to talk";
                worksheet.Cells[2, 9].Value = "Location";
            }

            using (var range = worksheet.Cells[1, 1, 2, 9])
            {
                range.Style.Font.Bold = true;
            }

            int currentRow = 3;

            foreach (var team in teams)
            {
                worksheet.Cells[currentRow, 1].Value = $"[Leader] {team.LeaderName}_Total";

                if (filterOptions.ViewType == TmrDataReportViewType.CallStatus)
                {
                    worksheet.Cells[currentRow, 4].Value = team.CallStatusCounts.tele;
                    worksheet.Cells[currentRow, 5].Value = team.CallStatusCounts.temp;
                    worksheet.Cells[currentRow, 6].Value = team.CallStatusCounts.notEnough;
                    worksheet.Cells[currentRow, 7].Value = team.CallStatusCounts.meet;
                    worksheet.Cells[currentRow, 8].Value = team.CallStatusCounts.refuse;
                    worksheet.Cells[currentRow, 9].Value = team.CallStatusCounts.location;
                }
                else
                {
                    worksheet.Cells[currentRow, 4].Value = team.TotalAssign;
                    worksheet.Cells[currentRow, 5].Value = team.TotalAvailableContact;
                }

                using (var leaderRowRange = worksheet.Cells[currentRow, 1, currentRow, filterOptions.ViewType == TmrDataReportViewType.CallStatus ? 9 : 5])
                {
                    leaderRowRange.Style.Font.Bold = true;
                    leaderRowRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    leaderRowRange.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                }

                currentRow++;

                foreach (var user in team.Users)
                {
                    worksheet.Cells[currentRow, 2].Value = user.UserName;
                    worksheet.Cells[currentRow, 3].Value = user.FullName;

                    if (filterOptions.ViewType == TmrDataReportViewType.CallStatus)
                    {
                        worksheet.Cells[currentRow, 4].Value = user.CallStatusCounts.tele;
                        worksheet.Cells[currentRow, 5].Value = user.CallStatusCounts.temp;
                        worksheet.Cells[currentRow, 6].Value = user.CallStatusCounts.notEnough;
                        worksheet.Cells[currentRow, 7].Value = user.CallStatusCounts.meet;
                        worksheet.Cells[currentRow, 8].Value = user.CallStatusCounts.refuse;
                        worksheet.Cells[currentRow, 9].Value = user.CallStatusCounts.location;
                    }
                    else
                    {
                        worksheet.Cells[currentRow, 4].Value = user.TotalAssign;
                        worksheet.Cells[currentRow, 5].Value = user.TotalAvailableContact;
                    }

                    currentRow++;
                }
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return TResult<byte[]?>.Ok(await package.GetAsByteArrayAsync());
        }
    }

    public async Task<TResult> GetTmrDataReportAsync(TmrDataReportFilterOptions filterOptions)
    {
        try
        {
            var callStatuses = await _context.CallStatuses.ToListAsync();

            int? GetStatusId(CallStatusType type) =>
                callStatuses.FirstOrDefault(x => x.Type == type)?.Id;

            var teleNotUpdateId = GetStatusId(CallStatusType.NO_PICK_UP);
            var tempLockedId = GetStatusId(CallStatusType.TEMPORARY_LOCKED);
            var notEnoughId = GetStatusId(CallStatusType.NOT_ENOUGH_QUALIFY);
            var meetRequireId = GetStatusId(CallStatusType.MEET_REQUIRE);
            var refuseId = GetStatusId(CallStatusType.CALL_LATER);
            var locationId = GetStatusId(CallStatusType.LOCATION);

            var data = await (from t in _context.Teams
                              join u in _context.Users on t.Id equals u.TeamId into tu
                              from u in tu.DefaultIfEmpty()
                              join c in _context.Contacts on u.Id equals c.UserId into uc
                              from c in uc.DefaultIfEmpty()
                              where filterOptions.TeamId == null || t.Id == filterOptions.TeamId
                              select new
                              {
                                  TeamId = t.Id,
                                  TeamName = t.Name,
                                  UserId = (Guid?)u.Id,
                                  UserName = u.UserName,
                                  FullName = u.Name,
                                  ContactId = (Guid?)c.Id,
                                  ContactStatus = (ContactStatus?)c.Status,
                                  CallStatusId = c.CallStatusId
                              }).ToListAsync();

            (int tele, int temp, int notEnough, int meet, int refuse, int location) GetCounts(IEnumerable<dynamic> items) =>
                (
                    tele: items.Count(x => x.ContactId != null && teleNotUpdateId.HasValue && x.CallStatusId == teleNotUpdateId),
                    temp: items.Count(x => x.ContactId != null && tempLockedId.HasValue && x.CallStatusId == tempLockedId),
                    notEnough: items.Count(x => x.ContactId != null && notEnoughId.HasValue && x.CallStatusId == notEnoughId),
                    meet: items.Count(x => x.ContactId != null && meetRequireId.HasValue && x.CallStatusId == meetRequireId),
                    refuse: items.Count(x => x.ContactId != null && refuseId.HasValue && x.CallStatusId == refuseId),
                    location: items.Count(x => x.ContactId != null && locationId.HasValue && x.CallStatusId == locationId)
                );

            var teams = data.GroupBy(x => new { x.TeamId, x.TeamName })
                .Select(g => new
                {
                    TeamId = g.Key.TeamId,
                    LeaderName = g.Key.TeamName,
                    TotalAssign = g.Count(x => x.ContactId != null),
                    TotalAvailableContact = g.Count(x => x.ContactId != null && x.ContactStatus != ContactStatus.Blacklisted),
                    CallStatusCounts = GetCounts(g),
                    Users = g.Where(x => x.UserId != null)
                        .GroupBy(x => new { x.UserId, x.UserName, x.FullName })
                        .Select(ug => new
                        {
                            UserId = ug.Key.UserId,
                            UserName = ug.Key.UserName,
                            FullName = ug.Key.FullName,
                            TotalAssign = ug.Count(x => x.ContactId != null),
                            TotalAvailableContact = ug.Count(x => x.ContactId != null && x.ContactStatus != ContactStatus.Blacklisted),
                            CallStatusCounts = GetCounts(ug)
                        }).ToList()
                }).ToList();

            return TResult.Ok(new
            {
                filterOptions.ViewType,
                Teams = teams
            });
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.ToString());
        }
    }

    public async Task<TResult<byte[]?>> ExportMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions)
    {
        try
        {
            var query = from t in _context.Teams
                        join s in _context.Sources on t.Id equals s.TeamId
                        join c in _context.Contacts on s.Id equals c.SourceId
                        join u in _context.Users on c.UserId equals u.Id
                        where c.UserId != null && c.Status != ContactStatus.Blacklisted
                        select new
                        {
                            t.Id,
                            TeamName = t.Name,
                            SourceName = s.Name,
                            TeleName = u.Name,
                            ContactId = c.Id,
                            c.LastCallTime,
                            c.ModifiedDate
                        };
            if (filterOptions.TeamId.HasValue)
            {
                query = query.Where(x => x.Id == filterOptions.TeamId);
            }
            if (filterOptions.FromDate.HasValue)
            {
                query = query.Where(x => x.ModifiedDate != null && x.ModifiedDate >= filterOptions.FromDate.Value);
            }
            if (filterOptions.ToDate.HasValue)
            {
                query = query.Where(x => x.ModifiedDate != null && x.ModifiedDate <= filterOptions.ToDate.Value);
            }
            var data = await query.GroupBy(x => new
            {
                x.Id,
                x.TeamName,
                x.SourceName,
                x.TeleName
            }).Select(x => new
            {
                x.Key.TeamName,
                x.Key.SourceName,
                x.Key.TeleName,
                TotalAssigned = x.Count(),
                TotalUsingAssigned = x.Count(c => c.LastCallTime != null),
                TotalRemainingAssigned = x.Count(c => c.LastCallTime == null)
            }).ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Multiple Assign Report");

            worksheet.Cells[1, 1].Value = "Team Name";
            worksheet.Cells[1, 2].Value = "Source Name";
            worksheet.Cells[1, 3].Value = "Tele Name";
            worksheet.Cells[1, 4].Value = "Total Assigned";
            worksheet.Cells[1, 5].Value = "Total Using Assigned";
            worksheet.Cells[1, 6].Value = "Total Remaining Assigned";

            using (var headerRange = worksheet.Cells[1, 1, 1, 6])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            int currentRow = 2;
            foreach (var item in data)
            {
                worksheet.Cells[currentRow, 1].Value = item.TeamName;
                worksheet.Cells[currentRow, 2].Value = item.SourceName;
                worksheet.Cells[currentRow, 3].Value = item.TeleName;
                worksheet.Cells[currentRow, 4].Value = item.TotalAssigned;
                worksheet.Cells[currentRow, 5].Value = item.TotalUsingAssigned;
                worksheet.Cells[currentRow, 6].Value = item.TotalRemainingAssigned;
                currentRow++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            using (var dataRange = worksheet.Cells[1, 1, currentRow - 1, 6])
            {
                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            return TResult<byte[]?>.Ok(await package.GetAsByteArrayAsync());
        }
        catch (Exception ex)
        {
            return TResult<byte[]?>.Failed(ex.Message);
        }
    }

    public async Task<TResult> GetMultipleAssignReportAsync(MultipleAssignReportFilterOptions filterOptions)
    {
        try
        {
            var query = from t in _context.Teams
                        join s in _context.Sources on t.Id equals s.TeamId
                        join c in _context.Contacts on s.Id equals c.SourceId
                        join u in _context.Users on c.UserId equals u.Id
                        where c.UserId != null && c.Status != ContactStatus.Blacklisted
                        select new
                        {
                            t.Id,
                            TeamName = t.Name,
                            SourceName = s.Name,
                            TeleName = u.Name,
                            ContactId = c.Id,
                            c.LastCallTime,
                            ModifiedDate = c.ModifiedDate ?? c.CreatedDate
                        };
            if (filterOptions.TeamId.HasValue)
            {
                query = query.Where(x => x.Id == filterOptions.TeamId);
            }
            if (filterOptions.FromDate.HasValue)
            {
                query = query.Where(x => x.ModifiedDate >= filterOptions.FromDate.Value);
            }
            if (filterOptions.ToDate.HasValue)
            {
                query = query.Where(x => x.ModifiedDate <= filterOptions.ToDate.Value);
            }
            var data = await query.GroupBy(x => new
            {
                x.Id,
                x.TeamName,
                x.SourceName,
                x.TeleName
            }).Select(x => new
            {
                x.Key.TeamName,
                x.Key.SourceName,
                TotalAssigned = x.Count(),
                TotalUsingAssigned = x.Count(c => c.LastCallTime != null),
                TotalRemainingAssigned = x.Count(c => c.LastCallTime == null)
            }).ToListAsync();

            return TResult.Ok(data);
        }
        catch (Exception ex)
        {
            return TResult.Failed(ex.Message);
        }
    }

    public async Task<Contact?> FindByPhoneNumberAsync(string phoneNumber) => await _context.Contacts.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
}
