using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IConfiguration _configuration;
    private readonly ILocalizationRepository _localizationRepository;
    private readonly IMemoryCache _memoryCache;

    public LocalizationService(IConfiguration configuration, ILocalizationRepository localizationRepository, IMemoryCache memoryCache)
    {
        _configuration = configuration;
        _localizationRepository = localizationRepository;
        _memoryCache = memoryCache;
    }

    public async Task<IdentityResult> AddAsync(Localization args)
    {
        if (await _localizationRepository.IsExistAsync(args.Language, args.Key))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Key existed!"
            });
        }
        await _localizationRepository.AddAsync(args);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var localization = await GetAsync(id);
        if (localization is null)
        {
            return IdentityResult.Failed();
        }
        await _localizationRepository.DeleteAsync(localization);
        return IdentityResult.Success;
    }

    public async Task<string> GetAsync(string key)
    {
        var cacheKey = $"{nameof(Localization)}-{key}";
        if (!_memoryCache.TryGetValue($"{cacheKey}", out string? cacheValue))
        {
            var lang = _configuration.GetValue<string>("language") ?? "vi-VN";
            var i18n = await _localizationRepository.FindAsync(key, lang);
            if (i18n is null)
            {
                i18n = new Localization
                {
                    Key = key,
                    Language = lang,
                };
                await _localizationRepository.AddAsync(i18n);
            }
            cacheValue = i18n.Value ?? key;
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
            _memoryCache.Set(cacheKey, cacheValue, cacheEntryOptions);
        }
        return cacheValue ?? key;
    }

    public async Task<Localization?> GetAsync(Guid id) => await _localizationRepository.FindAsync(id);

    public async Task<ListResult<Localization>> GetListAsync(LocalizationFilterOptions filterOptions)
    {
        var lang = filterOptions.Locale;
        var query = _localizationRepository.GetListAsync(lang, filterOptions.Key);
        return await ListResult<Localization>.Success(query, filterOptions);
    }

    public async Task<IdentityResult> SaveAsync(Localization args)
    {
        var localization = await GetAsync(args.Id);
        if (localization is null)
        {
            return IdentityResult.Failed();
        }
        localization.Value = args.Value;
        await _localizationRepository.SaveChangesAsync();
        return IdentityResult.Success;
    }
}
