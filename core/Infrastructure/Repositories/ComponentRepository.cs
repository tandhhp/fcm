using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories;

public class ComponentRepository : EfRepository<Component>, IComponentRepository
{
    public ComponentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Component?> FindByNameAsync(string normalizedName)
    {
        return await _context.Components.FirstOrDefaultAsync(x => x.NormalizedName.Equals(normalizedName) && x.Active);
    }

    public async Task<List<Component>> ListAsync(SearchFilterOptions filterOptions)
    {
        return await _context.Components.Where(x => (string.IsNullOrEmpty(filterOptions.SearchTerm) || x.NormalizedName.Contains(filterOptions.SearchTerm)) && x.Active).ToListAsync();
    }

    public async Task<ListResult<Component>> ListAsync(ComponentFilterOptions filterOptions)
    {
        var query = _context.Components.Where(x => string.IsNullOrEmpty(filterOptions.Name) || x.Name.ToLower().Contains(filterOptions.Name.ToLower()))
            .OrderBy(x => x.NormalizedName);
        return await ListResult<Component>.Success(query, filterOptions);
    }
}
