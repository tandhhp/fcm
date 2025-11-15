using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;

namespace Waffle.Infrastructure.Repositories;

public class LocalizationRepository : EfRepository<Localization>, ILocalizationRepository
{
    public LocalizationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Localization?> FindAsync(string key, string lang) => await _context.Localizations.FirstOrDefaultAsync(x => x.Key.ToLower().Equals(key.ToLower()) && x.Language.Equals(lang));

    public IQueryable<Localization> GetListAsync(string lang, string? key) => _context.Localizations.Where(x => x.Language.Equals(lang) && (string.IsNullOrEmpty(key) || x.Key.ToLower().Contains(key.ToLower()))).OrderBy(x => x.Key);

    public async Task<bool> IsExistAsync(string lang, string key) => await _context.Localizations.AnyAsync(x => x.Language.Equals(lang) && x.Key.Equals(key));

}
