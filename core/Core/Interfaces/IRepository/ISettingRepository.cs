using Waffle.Entities;

namespace Waffle.Core.Interfaces.IRepository;

public interface ISettingRepository : IAsyncRepository<AppSetting>
{
    Task<AppSetting?> FindByNameAsync(string normalizedName);
}
