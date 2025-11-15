using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Branches;

namespace Waffle.Infrastructure.Repositories;

public class BranchRepository(ApplicationDbContext context) : EfRepository<Branch>(context), IBranchRepository
{
    public async Task<ListResult<object>> ListAsync(BranchFilterOptions filterOptions)
    {
        var query = from a in _context.Branches
                    select new
                    {
                        a.Id,
                        a.Name,
                        DepartmentCount = _context.Departments.Count(d => d.BranchId == a.Id),
                    };
        if (!string.IsNullOrEmpty(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> OptionsAsync(SelectOptions selectOptions)
    {
        var query = from a in _context.Branches
                    select new
                    {
                        a.Id,
                        a.Name,
                    };
        if (!string.IsNullOrEmpty(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }
}
