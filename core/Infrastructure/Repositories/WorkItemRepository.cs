using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;

namespace Waffle.Infrastructure.Repositories;

public class WorkItemRepository : EfRepository<WorkContent>, IWorkContentRepository
{
    public WorkItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task AddItemAsync(Guid catalogId, Guid id)
    {
        await _context.WorkItems.AddAsync(new WorkItem
        {
            CatalogId = catalogId,
            WorkId = id
        });
        await _context.SaveChangesAsync();
    }

    public async Task<WorkContent?> FindByCatalogAsync(Guid catalogId,Guid componentId)
    {
        var query = from item in _context.WorkItems
                    join work in _context.WorkContents on item.WorkId equals work.Id
                    where item.CatalogId == catalogId && work.ComponentId == componentId
                    select work;
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<WorkContent>> ListChildAsync(Guid parentId) => await _context.WorkContents.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Guid>> ListChildIdAsync(Guid parentId) => await _context.WorkContents.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).Select(x => x.Id).ToListAsync();

    public async Task<IEnumerable<Guid>> ListTagIdsAsync(Guid id) => await _context.WorkItems.Where(x => x.WorkId == id).Select(x => x.CatalogId).ToListAsync();
}
