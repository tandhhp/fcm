using Microsoft.AspNetCore.Identity;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Core.Interfaces.IService
{
    public interface IComponentService
    {
        Task<Component> EnsureComponentAsync(string normalizedName);
        Task<Component?> GetByNameAsync(string name);
        Task<Component?> FindAsync(Guid id);
        Task<ListResult<Component>> ListAsync(ComponentFilterOptions filterOptions);
        Task<bool> HasWorkContentAsync(Guid id);
        Task<IdentityResult> ActiveAsync(Guid id);
        Task<IdentityResult> DeleteAsync(Guid id);
        Task<ListResult<WorkListItem>> ListWorkAsync(Guid id, WorkFilterOptions filterOptions);
        Task<IEnumerable<Component>> ListAllAsync();
        Task<IdentityResult> UpdateAsync(Component args);
        Task<IEnumerable<Option>> FormSelectAsync(SearchFilterOptions filterOptions);
        Task<IdentityResult> AddAsync(Component args);
    }
}
