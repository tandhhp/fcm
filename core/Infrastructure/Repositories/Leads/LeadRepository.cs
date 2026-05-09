using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Leads.Args;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Leads.Results;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Entities.Payments;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Leads;

public class LeadRepository(ApplicationDbContext context, IHCAService _hcaService) : EfRepository<Lead>(context), ILeadRepository
{
    public async Task AddFeedbackAsync(LeadFeedback feedback)
    {
        await _context.LeadFeedbacks.AddAsync(feedback);
        await _context.SaveChangesAsync();
    }

    public async Task AddSubLeadsAsync(Guid leadId, List<SubLeadCreateArgs> subLeads)
    {
        foreach (var subLead in subLeads)
        {
            await _context.SubLeads.AddAsync(new SubLead
            {
                LeadId = leadId,
                Name = subLead.Name,
                PhoneNumber = subLead.PhoneNumber,
                IdentityNumber = subLead.IdentityNumber,
                Gender = subLead.Gender
            });
        }
    }

    public async Task<IEnumerable<string?>> AllPhoneNumbersAsync() => await _context.Leads
        .Where(x => x.PhoneNumber != null)
        .Select(x => x.PhoneNumber).Distinct().ToListAsync();

    public async Task<ExistingLeadResult?> FindByIdentityNumberAsync(string identityNumber)
    {
        var query = from l in _context.Leads
                    join e in _context.Events on l.EventId equals e.Id
                    where l.IdentityNumber == identityNumber
                    select new ExistingLeadResult
                    {
                        EventDate = l.EventDate,
                        EventName = e.Name,
                        Id = l.Id,
                        Dupplicated = l.Duplicated
                    };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<Lead?> FindByPhoneNumberAsync(string? phoneNumber) => await _context.Leads.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

    public async Task<ListResult<object>> GetCheckinListAsync(LeadCheckinListFilterOptions filterOptions)
    {
        try
        {
            var userId = _hcaService.GetUserId();
            var query = from l in _context.Leads
                        join e in _context.Events on l.EventId equals e.Id
                        join c in _context.Users on l.CreatedBy equals c.Id
                        join s in _context.Sources on l.SourceId equals s.Id into sourceJoin
                        from s in sourceJoin.DefaultIfEmpty()
                        join sales in _context.Users on l.SalesId equals sales.Id into salesJoin
                        from sales in salesJoin.DefaultIfEmpty()
                        join telesales in _context.Users on l.TelesaleId equals telesales.Id into telesalesJoin
                        from telesales in telesalesJoin.DefaultIfEmpty()
                        join f in _context.LeadFeedbacks on l.Id equals f.LeadId into feedbackJoin
                        from f in feedbackJoin.DefaultIfEmpty()
                        join t in _context.Users on l.ToById equals t.Id into toJoin
                        from t in toJoin.DefaultIfEmpty()
                        where l.Status != LeadStatus.Pending && l.Status != LeadStatus.Approved && l.Status != LeadStatus.ReInvite
                        select new
                        {
                            l.Id,
                            l.Name,
                            l.PhoneNumber,
                            l.SalesId,
                            l.TelesaleId,
                            SalesName = sales.Name,
                            TelesalesName = telesales.Name,
                            l.SourceId,
                            SourceName = s.Name,
                            l.EventId,
                            EventName = e.Name,
                            l.EventDate,
                            l.Status,
                            l.Gender,
                            l.CreatedDate,
                            ToName = t.Name,
                            SalesManagerName = sales.SmId != null ? _context.Users.First(u => u.Id == sales.SmId).Name : null,
                            l.IdentityNumber,
                            l.DateOfBirth,
                            l.Note,
                            InviteCount = _context.LeadHistories.Count(h => h.LeadId == l.Id) + 1,
                            l.BranchId,
                            f.TableId,
                            TeamKeyIn = c.ManagerId != null ? _context.Users.First(x => x.Id == c.ManagerId).Name : null,
                            CreatorName = c.Name,
                            TmKeyIn = _context.Users.First(u => u.Id == c.TmId).Name,
                            SmKeyIn = _context.Users.First(u => u.Id == c.SmId).Name,
                            c.DotId,
                            c.DosId,
                            c.TmId,
                            c.SmId,
                            l.CreatedBy,
                            l.Duplicated,
                            l.AttendanceId,
                            SubLeads = _context.SubLeads.Where(x => x.LeadId == l.Id).Select(sub => $"{sub.Name} - {sub.PhoneNumber}").ToList(),
                            TeamSales = c.SmId != null ? _context.Users.First(u => u.Id == c.SmId).Name : null,
                            TeamTelesales = c.TmId != null ? _context.Users.First(u => u.Id == c.TmId).Name : null,
                            Voucher1 = _context.Vouchers.First(v => v.Id == l.Voucher1Id).Name,
                            Voucher2 = _context.Vouchers.First(v => v.Id == l.Voucher2Id).Name,
                        };
            if (!string.IsNullOrWhiteSpace(filterOptions.IdentityNumber))
            {
                query = query.Where(x => x.IdentityNumber != null && x.IdentityNumber.Contains(filterOptions.IdentityNumber));
            }
            if (filterOptions.EventId.HasValue)
            {
                query = query.Where(x => x.EventId == filterOptions.EventId);
            }
            if (filterOptions.EventDate != null)
            {
                query = query.Where(x => x.EventDate.Date == filterOptions.EventDate.Value.Date);
            }
            if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
            {
                query = from q in query
                        join sub in _context.SubLeads on q.Id equals sub.LeadId into subJoin
                        where q.PhoneNumber != null && q.PhoneNumber.Contains(filterOptions.PhoneNumber) ||
                              subJoin.Any(s => s.PhoneNumber != null && s.PhoneNumber.Contains(filterOptions.PhoneNumber))
                        select q;
            }
            if (!string.IsNullOrWhiteSpace(filterOptions.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
            }
            if (filterOptions.FromDate != null && filterOptions.ToDate != null)
            {
                query = query.Where(x => x.EventDate.Date >= filterOptions.FromDate.Value.Date && x.EventDate.Date <= filterOptions.ToDate.Value.Date);
            }
            if (filterOptions.Status.HasValue)
            {
                query = query.Where(x => x.Status == filterOptions.Status);
            }
            if (_hcaService.IsUserInRole(RoleName.Sales))
            {
                query = query.Where(x => x.SalesId == userId || x.CreatedBy == userId);
            }
            if (_hcaService.IsUserInRole(RoleName.Telesales))
            {
                query = query.Where(x => x.TelesaleId == userId || x.CreatedBy == userId);
            }
            if (_hcaService.IsUserInRole(RoleName.SalesManager))
            {
                query = query.Where(x => x.SmId == userId);
            }
            if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
            {
                query = query.Where(x => x.TmId == userId);
            }
            if (_hcaService.IsUserInRole(RoleName.DOS))
            {
                query = query.Where(x => x.DosId == userId);
            }
            if (_hcaService.IsUserInRole(RoleName.Dot))
            {
                query = query.Where(x => x.DotId == userId);
            }
            query = query.OrderByDescending(x => x.EventDate);
            return await ListResult<object>.Success(query, filterOptions);
        }
        catch (Exception ex)
        {
            return ListResult<object>.Failed(ex.ToString());
        }
    }

    public async Task<List<LeadExportCheckinResult>> GetExportCheckinDataAsync(LeadCheckinListFilterOptions filterOptions)
    {
        var query = from a in _context.Leads
                    join e in _context.Events on a.EventId equals e.Id
                    join f in _context.LeadFeedbacks on a.Id equals f.LeadId into feedbackJoin
                    from f in feedbackJoin.DefaultIfEmpty()
                    join s in _context.Sources on a.SourceId equals s.Id into sourceJoin
                    from s in sourceJoin.DefaultIfEmpty()
                    join sales in _context.Users on a.SalesId equals sales.Id into salesJoin
                    from sales in salesJoin.DefaultIfEmpty()
                    join creator in _context.Users on a.CreatedBy equals creator.Id
                    join contract in _context.Contracts on a.Id equals contract.LeadId into contractJoin
                    from contract in contractJoin.DefaultIfEmpty()
                    join attendance in _context.Attendances on a.AttendanceId equals attendance.Id into attendanceJoin
                    from attendance in attendanceJoin.DefaultIfEmpty()
                    join table in _context.Tables on f.TableId equals table.Id into tableJoin
                    from table in tableJoin.DefaultIfEmpty()
                    join to in _context.Users on a.ToById equals to.Id into toJoin
                    from to in toJoin.DefaultIfEmpty()
                    where a.Status != LeadStatus.Pending && a.Status != LeadStatus.Approved
                    select new LeadExportCheckinResult
                    {
                        ContractCode = contract.Code,
                        AmountPaid = _context.Invoices.Where(x => x.ContractId == contract.Id && x.Status == InvoiceStatus.Approved).Sum(x => x.Amount),
                        AttendanceName = attendance.Name,
                        CustomerIdNumber = a.IdentityNumber,
                        CustomerName = a.Name,
                        CustomerPhoneNumber = a.PhoneNumber,
                        DateOfBirth = a.DateOfBirth,
                        EventDate = a.EventDate,
                        EventName = e.Name,
                        SalesManagerName = sales != null ? _context.Users.First(x => x.SmId == sales.Id).Name : null,
                        SourceName = s.Name,
                        TableName = table.Name,
                        SalesName = sales.Name,
                        ToName = to.Name,
                        TotalAmount = contract.Amount,
                        KeyInName = creator.Name,
                        EventId = e.Id,
                        TeamKeyIn = creator.ManagerId != null ? _context.Users.First(u => u.Id == creator.ManagerId).Name : null,
                        DOS = creator.DosId != null ? _context.Users.First(u => u.Id == creator.DosId).Name : null,
                    };
        if (filterOptions.FromDate != null && filterOptions.ToDate != null)
        {
            query = query.Where(x => x.EventDate.Date >= filterOptions.FromDate.Value.Date && x.EventDate.Date <= filterOptions.ToDate.Value.Date);
        }
        if (filterOptions.EventId.HasValue)
        {
            query = query.Where(x => x.EventId == filterOptions.EventId);
        }
        if (filterOptions.EventDate != null)
        {
            query = query.Where(x => x.EventDate.Date == filterOptions.EventDate.Value.Date);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => x.CustomerPhoneNumber != null && x.CustomerPhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.CustomerName != null && x.CustomerName.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.Status.HasValue)
        {
            query = query.Where(x => x.Status == filterOptions.Status);
        }
        query = query.OrderByDescending(x => x.EventDate);
        return await query.ToListAsync();
    }

    public async Task<LeadFeedback?> GetFeedbackAsync(Guid id) => await _context.LeadFeedbacks.FirstOrDefaultAsync(x => x.LeadId == id);

    public async Task<object> GetSubLeadsAsync(Guid id) => await _context.SubLeads.Where(x => x.LeadId == id).Select(x => new
    {
        x.Id,
        x.Name,
        x.PhoneNumber,
        x.Gender,
        x.IdentityNumber
    }).ToListAsync();

    public async Task<ListResult<object>> GetWaitingListAsync(LeadWaittingListFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return ListResult<object>.Failed("User not found");

        var query = from l in _context.Leads
                    join e in _context.Events on l.EventId equals e.Id
                    join s in _context.Sources on l.SourceId equals s.Id into sourceJoin
                    from s in sourceJoin.DefaultIfEmpty()
                    join sales in _context.Users on l.SalesId equals sales.Id into salesJoin
                    from sales in salesJoin.DefaultIfEmpty()
                    join telesales in _context.Users on l.TelesaleId equals telesales.Id into telesalesJoin
                    from telesales in telesalesJoin.DefaultIfEmpty()
                    join c in _context.Users on l.CreatedBy equals c.Id
                    where l.Status == LeadStatus.Pending || l.Status == LeadStatus.Approved
                    select new
                    {
                        l.Id,
                        l.Name,
                        l.PhoneNumber,
                        l.SalesId,
                        l.TelesaleId,
                        SalesName = sales.Name,
                        TelesalesName = telesales.Name,
                        l.SourceId,
                        SourceName = s.Name,
                        l.EventId,
                        EventName = e.Name,
                        l.EventDate,
                        l.Status,
                        l.Gender,
                        l.CreatedDate,
                        l.CreatedBy,
                        c.SmId,
                        c.TmId,
                        c.DosId,
                        c.DotId,
                        CreatorName = c.Name,
                        SubLeads = _context.SubLeads.Where(x => x.LeadId == l.Id).Select(sub => $"{sub.Name} - {sub.PhoneNumber}").ToList(),
                        l.BranchId,
                        c.Avatar,
                        l.Confirm2
                    };
        if (filterOptions.EventId.HasValue)
        {
            query = query.Where(x => x.EventId == filterOptions.EventId);
        }
        if (filterOptions.EventDate != null)
        {
            query = query.Where(x => x.EventDate.Date == filterOptions.EventDate.Value.Date);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = query.Where(x => x.PhoneNumber != null && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (_hcaService.IsUserInRole(RoleName.Sales))
        {
            query = query.Where(x => x.SalesId == userId || x.CreatedBy == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Telesales))
        {
            query = query.Where(x => x.TelesaleId == userId || x.CreatedBy == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.SalesManager))
        {
            query = query.Where(x => x.SmId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.TelesaleManager))
        {
            query = query.Where(x => x.TmId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.DOS))
        {
            query = query.Where(x => x.DosId == userId);
        }
        if (_hcaService.IsUserInRole(RoleName.Event))
        {
            query = query.Where(x => x.BranchId == user.BranchId);
        }
        if (filterOptions.BranchId.HasValue)
        {
            query = query.Where(x => x.BranchId == filterOptions.BranchId);
        }
        query = query.OrderByDescending(x => x.EventDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<bool> IsCitizenIdExistAsync(string? citizenId, Guid leadId)
    {
        if (string.IsNullOrWhiteSpace(citizenId)) return false;
        return await _context.Leads.AnyAsync(x => x.IdentityNumber == citizenId && leadId != x.Id);
    }

    public Task<bool> IsPhoneExistAsync(string phoneNumber, Guid id)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)) return Task.FromResult(false);
        return _context.Leads.AnyAsync(x => x.PhoneNumber == phoneNumber && id != x.Id);
    }

    public async Task<List<Lead>> ListByCitizenIdAsync(string citizenId)
    {
        return await _context.Leads.Where(x => x.IdentityNumber == citizenId).ToListAsync();
    }

    public async Task<ListResult<object>> NoDupsAsync(LeadCheckinListFilterOptions filterOptions)
    {
        var query = from l in _context.Leads
                    where l.Status != LeadStatus.Pending && l.Status != LeadStatus.Approved
                    orderby l.CreatedDate descending
                    select new
                    {
                        l.Id,
                        l.Name,
                        l.PhoneNumber,
                        l.IdentityNumber,
                        l.DateOfBirth,
                        l.Note,
                        l.Duplicated,
                        Count = _context.Leads.Count(x => !string.IsNullOrEmpty(l.PhoneNumber) && !string.IsNullOrEmpty(x.IdentityNumber) && (x.PhoneNumber == l.PhoneNumber || x.IdentityNumber == l.IdentityNumber))
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.PhoneNumber))
        {
            query = from l in query
                    join sub in _context.SubLeads on l.Id equals sub.LeadId into subJoin
                    where l.PhoneNumber != null && l.PhoneNumber.Contains(filterOptions.PhoneNumber) ||
                          subJoin.Any(s => s.PhoneNumber != null && s.PhoneNumber.Contains(filterOptions.PhoneNumber))
                    select l;
            query = query.Where(x => x.PhoneNumber != null && x.PhoneNumber.Contains(filterOptions.PhoneNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.IdentityNumber))
        {
            query = from l in query
                    join sub in _context.SubLeads on l.Id equals sub.LeadId into subJoin
                    where l.IdentityNumber != null && l.IdentityNumber.Contains(filterOptions.IdentityNumber) ||
                          subJoin.Any(s => s.IdentityNumber != null && s.IdentityNumber.Contains(filterOptions.IdentityNumber))
                    select l;
            query = query.Where(x => x.IdentityNumber != null && x.IdentityNumber.Contains(filterOptions.IdentityNumber));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        var groups = await query.Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync();
        var leadIds = groups.Select(x => x.Id).ToList();
        var subLeads = await _context.SubLeads.Where(x => leadIds.Contains(x.LeadId)).ToListAsync();
        var data = new List<LeadCustomer>();
        foreach (var item in groups)
        {
            data.Add(new LeadCustomer
            {
                Id = item.Id,
                IdentityNumber = item.IdentityNumber,
                Name = item.Name,
                PhoneNumber = item.PhoneNumber,
                DateOfBirth = item.DateOfBirth,
                Note = item.Note,
                Count = item.Count,
                Duplicated = item.Duplicated,
                SubLeads = subLeads.Where(x => x.LeadId == item.Id).Select(sub => new SubLeadCustomer
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    PhoneNumber = sub.PhoneNumber,
                    Gender = sub.Gender,
                    IdentityNumber = sub.IdentityNumber
                })
            });
        }
        return new ListResult<object>(data, await query.CountAsync(), filterOptions);
    }

    public async Task<LeadFeedback> SaveFeedbackAsync(Guid leadId, int? tableId)
    {
        var feedback = await _context.LeadFeedbacks.FirstOrDefaultAsync(x => x.LeadId == leadId);
        if (feedback is null)
        {
            feedback = new LeadFeedback
            {
                LeadId = leadId,
                CheckinTime = DateTime.Now.TimeOfDay,
                TableId = tableId
            };
            await _context.LeadFeedbacks.AddAsync(feedback);
        }
        return feedback;
    }

    public async Task UpdateFeedbackAsync(Lead lead, LeadFeedback feedback)
    {
        _context.LeadFeedbacks.Update(feedback);
        _context.Leads.Update(lead);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSubLeadsAsync(Guid id, List<SubLeadUpdateArgs> subLeads)
    {
        var existingSubLeads = await _context.SubLeads.Where(x => x.LeadId == id).ToListAsync();
        if (existingSubLeads.Count > 0)
        {
            _context.SubLeads.RemoveRange(existingSubLeads);
        }
        foreach (var subLead in subLeads)
        {
            await _context.SubLeads.AddAsync(new SubLead
            {
                LeadId = id,
                Name = subLead.Name,
                PhoneNumber = subLead.PhoneNumber,
                Gender = subLead.Gender,
                IdentityNumber = subLead.IdentityNumber
            });
        }
    }
}
