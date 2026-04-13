using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Teams.Models;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Teams;

public class GroupDataRepository(ApplicationDbContext context) : EfRepository<GroupData>(context), IGroupDataRepository
{
    public async Task<bool> ExistsAsync(string name, int? excludedId = null)
    {
        var normalizedName = name.Trim().ToLower();
        return await _context.GroupDatas.AnyAsync(x => x.Name.ToLower() == normalizedName && (!excludedId.HasValue || x.Id != excludedId.Value));
    }

    public async Task<bool> HasTeamAsync(int id) => await _context.Teams.AnyAsync(x => x.GroupDataId == id);

    public async Task<ListResult<object>> ListAsync(GroupDataFilterOptions filterOptions)
    {
        var query = from g in _context.GroupDatas
                    select new
                    {
                        g.Id,
                        g.Name,
                        TeamCount = _context.Teams.Count(t => t.GroupDataId == g.Id)
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderBy(x => x.Name);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object> GetOptionsAsync(SelectOptions selectOptions)
    {
        var query = from a in _context.GroupDatas
                    select new
                    {
                        a.Id,
                        a.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
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
