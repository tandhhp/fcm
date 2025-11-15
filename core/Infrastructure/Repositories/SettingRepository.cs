using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;

namespace Waffle.Infrastructure.Repositories;

public class SettingRepository(ApplicationDbContext context) : EfRepository<AppSetting>(context), ISettingRepository
{
    public async Task<AppSetting?> FindByNameAsync(string normalizedName)
    {
        if (string.IsNullOrWhiteSpace(normalizedName)) return default;
        return await _context.AppSettings.FirstOrDefaultAsync(x => x.NormalizedName == normalizedName);
    }
}
