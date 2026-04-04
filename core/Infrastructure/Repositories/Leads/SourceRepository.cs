using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Leads;

public class SourceRepository(ApplicationDbContext context) : EfRepository<Source>(context), ISourceRepository
{
    public async Task<TResult> AssignAsync(SourceAssignArgs args)
    {
        if (args.Sources is null || !args.Sources.Any())
            return TResult.Failed("Chưa chọn nguồn để gán!");
        if (args.Assigns is null || !args.Assigns.Any())
            return TResult.Failed("Chưa chọn telesales để gán!");

        // Get contacts eligible for assignment: unassigned and from selected sources
        var contacts = await _context.Contacts
            .Where(x => x.UserId == null)
            .Where(x => x.SourceId != null && args.Sources.Contains(x.SourceId.Value))
            .OrderBy(x => x.Id)
            .ToListAsync();

        if (!contacts.Any()) return TResult.Failed("Không có contact để gán.");

        int totalAssigned = 0;

        foreach (var assign in args.Assigns)
        {
            if (contacts.Count == 0) break;

            int quantity = assign.NumberOfContact;

            var take = contacts.Take(quantity).ToList();
            foreach (var c in take)
            {
                c.UserId = assign.TelesalesId;
            }

            totalAssigned += take.Count;
            contacts = contacts.Skip(take.Count).ToList();
        }

        if (totalAssigned == 0)
            return TResult.Failed("Không có contact để gán.");

        await _context.SaveChangesAsync();
        return TResult.Success;
    }

    public async Task<TResult<object>> AvailablesAsync()
    {
        var query = from s in _context.Sources
                    select new
                    {
                        Value = s.Id,
                        Label = s.Name,
                        ContactCount = _context.Contacts.Count(c => c.SourceId == s.Id)
                    };
        var data = await query.ToListAsync();
        return TResult<object>.Ok(data);
    }

    public async Task<TypeOfData?> GetTypeOfDataByIdAsync(int? typeOfDataId) => await _context.TypeOfDatas.FindAsync(typeOfDataId);

    public async Task<TResult> GetTypeOfDataBySourceIdAsync(int sourceId)
    {
        var query = from s in _context.Sources
                    join t in _context.TypeOfDatas on s.TypeOfDataId equals t.Id
                    where s.Id == sourceId
                    select t;
        return TResult.Ok(await query.FirstOrDefaultAsync());
    }

    public async Task<bool> IsExistAsync(string name) => await _context.Sources.AnyAsync(x => x.Name == name);

    public Task<bool> IsUsedAsync(int id) => _context.Leads.AnyAsync(x => x.SourceId == id);

    public async Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions)
    {
        var query = from s in _context.Sources
                    join t in _context.TypeOfDatas on s.TypeOfDataId equals t.Id into st
                    from t in st.DefaultIfEmpty()
                    join g in _context.Teams on s.TeamId equals g.Id into sg
                    from g in sg.DefaultIfEmpty()
                    select new SourceListItem
                    {
                        Id = s.Id,
                        Name = s.Name,
                        TypeOfData = t.Name,
                        TypeOfDataId = s.TypeOfDataId,
                        SourceType = t.Source,
                        Overwrite = s.Overwrite,
                        Protected = s.Protected,
                        ContactCount = _context.Contacts.Count(c => c.SourceId == s.Id),
                        TeamName = g.Name,
                        TeamId = s.TeamId
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        if (filterOptions.SourceType.HasValue)
        {
            query = query.Where(x => x.SourceType == filterOptions.SourceType);
        }
        if (filterOptions.TypeOfDataId.HasValue)
        {
            query = query.Where(x => x.TypeOfDataId == filterOptions.TypeOfDataId);
        }
        if (filterOptions.TeamId.HasValue)
        {
            query = query.Where(x => x.TeamId == filterOptions.TeamId);
        }
        query = query.OrderByDescending(x => x.Id);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object> OptionsAsync(SourceSelectOptions selectOptions)
    {
        var query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name,
                        s.TeamId
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        if (selectOptions.TeamId.HasValue)
        {
            query = query.Where(x => x.TeamId == selectOptions.TeamId);
        }
        query = query.OrderByDescending(x => x.Id);
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<object?> OptionsByTypeOfDataAsync(SourceSelectOptions selectOptions)
    {
        var query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name,
                        s.TeamId,
                        ContactCount = (from c in _context.Contacts.Where(x => x.SourceId == s.Id)
                                        where c.UserId != null
                                        where c.Status != ContactStatus.Blacklisted
                                        select c.Id).Count()
                    };
        if (selectOptions.TypeOfData == TypeOfDataSelectType.New)
        {
            query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name,
                        s.TeamId,
                        ContactCount = (from c in _context.Contacts.Where(x => x.SourceId == s.Id)
                                        where c.Status != ContactStatus.Blacklisted
                                        where c.UserId == null
                                        select c.Id).Count()
                    };
        }
        if (selectOptions.TypeOfData == TypeOfDataSelectType.Old)
        {
            query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name,
                        s.TeamId,
                        ContactCount = (from c in _context.Contacts.Where(x => x.SourceId == s.Id)
                                        where c.UserId != null
                                        where c.Status != ContactStatus.Blacklisted
                                        where c.CreatedDate < DateTime.UtcNow.AddMonths(-1)
                                        select c.Id).Count()
                    };
        }
        query = query.Where(x => x.TeamId == selectOptions.TeamId);
        return await query.OrderByDescending(x => x.ContactCount).Select(x => new
        {
            Label = $"{x.Name} ({x.ContactCount})",
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<ListResult<SourceReportResult>> ReportAsync(FilterOptions filterOptions)
    {
        var query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name
                    };
        var data = await query.Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync();
        var result = new List<SourceReportResult>();
        foreach (var item in data)
        {
            var leadCount = await _context.Leads.CountAsync(x => x.SourceId == item.Id);
            result.Add(new SourceReportResult
            {
                SourceName = item.Name,
                ContactCount = await _context.Contacts.CountAsync(x => x.SourceId == item.Id),
                CallCount = await (from c in _context.Contacts.Where(x => x.SourceId == item.Id)
                                   join h in _context.CallHistories on c.Id equals h.ContactId
                                   select h.Id).CountAsync(),
            });
        }
        return new ListResult<SourceReportResult>(result, await query.CountAsync(), filterOptions);
    }

    public async Task<object?> TeamOptionsAsync(SourceTeamSelectOptions selectOptions)
    {
        var query = from t in _context.Teams
                    select new
                    {
                        t.Id,
                        t.Name,
                        ContactCount = (from s in _context.Sources.Where(s => s.TeamId == t.Id)
                                        join c in _context.Contacts on s.Id equals c.SourceId
                                        where c.UserId != null
                                        select c.Id).Count()
                    };
        if (selectOptions.TypeOfData == TypeOfDataSelectType.New)
        {
            // new: là đẩy dữ liệu lên và chưa chia, chưa dùng tới
            query = from t in _context.Teams
                    select new
                    {
                        t.Id,
                        t.Name,
                        ContactCount = (from s in _context.Sources.Where(s => s.TeamId == t.Id)
                                        join c in _context.Contacts on s.Id equals c.SourceId
                                        where c.UserId == null
                                        select c.Id).Count()
                    };
        }
        if (selectOptions.TypeOfData == TypeOfDataSelectType.Old)
        {
            // old là đã chia rồi và mốc sẽ là 1 tháng trở về trước
            query = from t in _context.Teams
                    select new
                    {
                        t.Id,
                        t.Name,
                        ContactCount = (from s in _context.Sources.Where(s => s.TeamId == t.Id)
                                        join c in _context.Contacts on s.Id equals c.SourceId
                                        where c.UserId != null
                                        where c.CreatedDate < DateTime.UtcNow.AddMonths(-1)
                                        select c.Id).Count()
                    };
        }
        return await query.OrderByDescending(x => x.ContactCount).Select(x => new
        {
            Label = $"{x.Name} ({x.ContactCount})",
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<object?> TypeOfDataOptionsAsync(TypeOfDataSelectOptions selectOptions)
    {
        var query = from t in _context.TypeOfDatas
                    select new
                    {
                        t.Id,
                        t.Name,
                        t.Source
                    };
        if (selectOptions.SourceType.HasValue)
        {
            query = query.Where(x => x.Source == selectOptions.SourceType);
        }
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.OrderByDescending(x => x.Id).Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }
}
