using Waffle.Entities;

namespace Waffle.Core.Interfaces.IRepository;

public interface IWorkContentRepository : IAsyncRepository<WorkContent>
{
    Task AddItemAsync(Guid catalogId, Guid id);
    Task<WorkContent?> FindByCatalogAsync(Guid catalogId, Guid componentId);
    Task<IEnumerable<WorkContent>> ListChildAsync(Guid parentId);
    Task<IEnumerable<Guid>> ListChildIdAsync(Guid parentId);
    Task<IEnumerable<Guid>> ListTagIdsAsync(Guid id);
}
