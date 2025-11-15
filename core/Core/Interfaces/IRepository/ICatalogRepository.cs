using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Core.Interfaces.IRepository;

public interface ICatalogRepository : IAsyncRepository<Catalog>
{
    Task<int> CountAsync(CatalogType type);
    Task<Catalog?> FindAsync(Guid catalogId, CatalogType type);
    Task<Catalog?> FindByNameAsync(string? normalizedName);
    Task<dynamic> GetComponentsAsync(Guid id);
    Task<IEnumerable<Option>> GetFormSelectAsync(SelectFilterOptions filterOptions);
    Task<object?> GetStructureAsync(Guid id);
    Task<object?> GetStructureAsync(string normalizedName);
    Task<IEnumerable<Catalog>> GetTopViewAsync(CatalogType type);
    Task<int> GetViewCountAsync();
    Task<ListResult<Catalog>> ListAsync(CatalogFilterOptions filterOptions);
    Task<IEnumerable<Catalog>> ListSpotlightAsync(CatalogType type, int pageSize);
}
