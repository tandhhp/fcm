using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Contacts.Args;
using Waffle.Core.Services.Contacts.Filters;
using Waffle.Core.Services.Contacts.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;
using Waffle.Models.Filters;

namespace Waffle.Infrastructure.Repositories;

public class ContactRepository(ApplicationDbContext context, IHCAService _hcaService, UserManager<ApplicationUser> _userManager) : EfRepository<Contact>(context), IContactRepository
{
    public async Task<ListResult<object>> DialedCallsAsync(ContactFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
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
            query = query.Where(c => c.CalledAt.Date >= filterOptions.FromDate.Value.Date && c.CalledAt.Date <= filterOptions.ToDate.Value.Date);
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
        if (!string.IsNullOrWhiteSpace(filterOptions.Note))
        {
            query = query.Where(x => x.Note != null && x.Note.ToLower().Contains(filterOptions.Note.ToLower()));
        }
        if (_hcaService.IsUserInRole(RoleName.Telesale))
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
                    where a.Status != ContactStatus.Blacklisted && a.UserId != null
                    select new
                    {
                        a.Id,
                        a.PhoneNumber,
                        a.Email,
                        a.CreatedDate,
                        a.Name,
                        a.Note,
                        TelesalesId = a.UserId,
                        TelesalesName = b.Name,
                        a.Gender,
                        IsBooked = _context.Leads.Any(x => x.PhoneNumber == a.PhoneNumber),
                        a.SourceId,
                        b.TmId,
                        b.DotId,
                        b.DosId,
                        a.Confirm1,
                        SourceName = s.Name,
                        IsCalled = _context.CallHistories.Any(x => x.ContactId == a.Id)
                    };
        query = query.Where(x => !x.IsCalled);
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToLower().Contains(filterOptions.PhoneNumber.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Email))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(filterOptions.Email.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.IsBooked.HasValue)
        {
            query = query.Where(x => x.IsBooked == filterOptions.IsBooked);
        }
        if (filterOptions.Confirm1.HasValue)
        {
            query = query.Where(x => x.Confirm1 == filterOptions.Confirm1);
        }
        if (_hcaService.IsUserInRole(RoleName.Telesale))
        {
            query = query.Where(x => x.TelesalesId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
        {
            query = query.Where(x => x.TmId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Dot))
        {
            query = query.Where(x => x.DotId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Dos))
        {
            query = query.Where(x => x.DosId == userId);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<dynamic>.Success(query, filterOptions);
    }

    public async Task<ListResult<object>> NeedConfirmsAsync(ContactFilterOptions filterOptions)
    {
        var query = from a in _context.Contacts
                    join b in _context.Users on a.UserId equals b.Id
                    join c in _context.Leads on a.PhoneNumber equals c.PhoneNumber
                    join d in _context.Events on c.EventId equals d.Id
                    where a.Status != ContactStatus.Blacklisted
                    select new
                    {
                        a.Id,
                        a.PhoneNumber,
                        a.Email,
                        a.CreatedDate,
                        a.Name,
                        a.Note,
                        TelesalesId = a.UserId,
                        TelesalesName = b.Name,
                        CallCount = _context.CallHistories.Count(x => x.ContactId == a.Id) + 1,
                        a.Confirm2Status,
                        c.EventDate,
                        EventName = d.Name,
                        b.TmId
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Email.Contains(filterOptions.Name));
        }
        if (filterOptions.Confirm2Status.HasValue)
        {
            query = query.Where(x => x.Confirm2Status == filterOptions.Confirm2Status);
        }
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(x => x.EventDate.Date >= filterOptions.FromDate.Value.Date && x.EventDate.Date <= filterOptions.ToDate.Value.Date);
        }
        if (_hcaService.IsUserInRole(RoleName.Telesale))
        {
            var user = await _userManager.FindByIdAsync(_hcaService.GetUserId().ToString());
            if (user is null) return ListResult<object>.Failed("User not found!");
            var claims = await _userManager.GetClaimsAsync(user);
            var hasAccessAll = claims.Any(c => c.Type == "ACCESS" && c.Value == "CONFIRM2");
            query = query.Where(x => hasAccessAll || x.TelesalesId == _hcaService.GetUserId());
        }
        if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
        {
            query = query.Where(x => x.TmId == _hcaService.GetUserId());
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public void Update(Contact contact) => _context.Contacts.Update(contact);
}
