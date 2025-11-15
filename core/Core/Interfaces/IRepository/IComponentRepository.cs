using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IComponentRepository : IAsyncRepository<Component>
{
    Task<Component?> FindByNameAsync(string normalizedName);
    Task<List<Component>> ListAsync(SearchFilterOptions filterOptions);
    Task<ListResult<Component>> ListAsync(ComponentFilterOptions filterOptions);
}
