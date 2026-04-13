using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.JobKinds.Models;
using Waffle.Data;
using Waffle.Entities.Users;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class JobKindRepository(ApplicationDbContext context) : EfRepository<JobKind>(context), IJobKindRepository
{
    public async Task<bool> IsUsedAsync(int id) => await _context.Contacts.AnyAsync(x => x.JobKindId == id);

    public async Task<ListResult<object>> ListAsync(JobKindFilterOptions filterOptions)
    {
        var query = from jk in _context.JobKinds
                    select new
                    {
                        jk.Id,
                        jk.Name,
                        jk.IsActive,
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderBy(x => x.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object?> OptionsAsync() => await _context.JobKinds
        .Where(x => x.IsActive)
        .OrderBy(x => x.Name)
        .Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
}
