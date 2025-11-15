using Microsoft.AspNetCore.Identity;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface ILocalizationService
{
    Task<IdentityResult> AddAsync(Localization args);
    Task<IdentityResult> DeleteAsync(Guid id);
    Task<string> GetAsync(string key);
    Task<Localization?> GetAsync(Guid id);
    Task<ListResult<Localization>> GetListAsync(LocalizationFilterOptions filterOptions);
    Task<IdentityResult> SaveAsync(Localization args);
}
