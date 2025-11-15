using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Services.Attendances.Filters;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Events;

public class AttendanceRepository(ApplicationDbContext context) : EfRepository<Attendance>(context), IAttendanceRepository
{
    public async Task<object> GetOptionsAsync(SelectOptions selectOptions)
    {
        var query = from a in _context.Attendances
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.SortOrder,
                        LeadCount = _context.Leads.Count(x => x.AttendanceId == a.Id)
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        query = query.OrderBy(x => x.SortOrder);
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<ListResult<object>> ListAsync(AttendanceFilterOptions filterOptions)
    {
        var query = from a in _context.Attendances
                    select new
                    {
                        a.SortOrder,
                        a.SuRate,
                        a.Name,
                        a.IsActive
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderBy(x => x.SortOrder);
        return await ListResult<object>.Success(query, filterOptions);
    }
}
