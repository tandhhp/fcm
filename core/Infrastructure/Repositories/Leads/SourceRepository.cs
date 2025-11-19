using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Leads;
using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Core.Services.Sources.Results;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;
using System.Reflection;

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
                        ContactCount = _context.Contacts.Count(c => c.SourceId == s.Id && c.UserId == null)
                    };
        var data = await query.Where(x => x.ContactCount > 0).ToListAsync();
        return TResult<object>.Ok(data);
    }

    public async Task<bool> IsExistAsync(string name) => await _context.Sources.AnyAsync(x => x.Name == name);

    public Task<bool> IsUsedAsync(int id) => _context.Leads.AnyAsync(x => x.SourceId == id);

    public async Task<ListResult<object>> ListAsync(SourceFilterOptions filterOptions)
    {
        var query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name,
                        LeadCount = _context.Leads.Count(l => l.SourceId == s.Id),
                        ContactCount = _context.Contacts.Count(c => c.SourceId == s.Id),
                        DialedCount = (from c in _context.Contacts.Where(c => c.SourceId == s.Id)
                                       join h in _context.CallHistories on c.Id equals h.ContactId
                                       select c.Id).Distinct().Count(),
                        AssignedCount = _context.Contacts.Count(c => c.SourceId == s.Id && c.UserId != null)
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderByDescending(x => x.Id);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object> OptionsAsync(SelectOptions selectOptions)
    {
        var query = from s in _context.Sources
                    select new
                    {
                        s.Id,
                        s.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        query = query.OrderByDescending(x => x.Id);
        return await query.Select(x => new
        {
            Label = x.Name,
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
}
