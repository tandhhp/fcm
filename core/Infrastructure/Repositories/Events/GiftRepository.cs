using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Events;

public class GiftRepository(ApplicationDbContext context) : EfRepository<Gift>(context), IGiftRepository
{
    public async Task<object?> GetOptionsAsync() => await _context.Gifts.Select(x => new
    {
        Label = x.Name,
        Value = x.Id
    }).ToListAsync();

    public async Task<ListResult<object>> ListAsync(GiftFilterOptions filterOptions)
    {
        var query = from a in _context.Gifts
                    select new
                    {
                        a.Id,
                        a.Name
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderByDescending(x => x.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }
}
