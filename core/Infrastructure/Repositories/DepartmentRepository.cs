using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Departments.Models;
using Waffle.Data;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext context) : EfRepository<Department>(context), IDepartmentRepository
{
    public async Task<bool> HasTeamAsync(int id) => await _context.Teams.AnyAsync(t => t.DepartmentId == id);

    public async Task<ListResult<object>> ListAsync(DepartmentFilterOptions filterOptions)
    {
        var query = from d in _context.Departments
                    join e in _context.Users on d.LeaderId equals e.Id into de
                    from e in de.DefaultIfEmpty()
                    select new
                    {
                        d.Id,
                        d.BranchId,
                        d.Name,
                        TeamCount = _context.Teams.Count(t => t.DepartmentId == d.Id),
                        LeaderName = e.Name,
                        d.LeaderId
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(d => d.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderBy(d => d.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> OptionsAsync(SelectOptions selectOptions)
    {
        var query = from d in _context.Departments
                    select new
                    {
                        d.Id,
                        d.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(d => d.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        query = query.OrderBy(d => d.Name);
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }
}
