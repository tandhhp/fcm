using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Teams;

public class GroupDataRepository(ApplicationDbContext context) : EfRepository<GroupData>(context), IGroupDataRepository
{
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
