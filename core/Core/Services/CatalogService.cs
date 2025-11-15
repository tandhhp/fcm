using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Options;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Args.Catalogs;
using Waffle.Models.Components;
using Waffle.Models.ViewModels;

namespace Waffle.Core.Services;

public class CatalogService(ApplicationDbContext context, IHCAService _hcaService, ICatalogRepository catalogRepository, IComponentRepository componentRepository, IWorkContentRepository workContentRepository, IOptions<SettingOptions> options) : ICatalogService
{
    private readonly ApplicationDbContext _context = context;
    private readonly ICatalogRepository _catalogRepository = catalogRepository;
    private readonly IComponentRepository _componentRepository = componentRepository;
    private readonly IWorkContentRepository _workRepository = workContentRepository;
    private readonly SettingOptions _options = options.Value;

    public async Task<IdentityResult> ActiveAsync(Guid id)
    {
        var catalog = await _catalogRepository.FindAsync(id);
        if (catalog is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.catalogNotFound",
                Description = "Catalog not found!"
            });
        }
        catalog.Active = !catalog.Active;
        catalog.ModifiedDate = DateTime.Now;
        await _catalogRepository.SaveChangesAsync();
        return IdentityResult.Success;
    }

    private async Task<bool> IsExistAsync(string normalizedName) => await _context.Catalogs.AnyAsync(x => x.NormalizedName.Equals(normalizedName));

    public async Task<IdentityResult> AddAsync(Catalog catalog)
    {
        if (string.IsNullOrEmpty(catalog.NormalizedName))
        {
            catalog.NormalizedName = SeoHelper.ToSeoFriendly(catalog.Name);
        }
        if (await IsExistAsync(catalog.NormalizedName))
        {
            if (catalog.Type == CatalogType.Tag)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "error.dataDupplicate",
                    Description = "Tag was existed!"
                });
            }
            var count = await _context.Catalogs.CountAsync(x => x.NormalizedName.Contains(catalog.NormalizedName));
            catalog.NormalizedName += $"-{count + 1}";
        }
        if (catalog.ParentId != null)
        {
            var parent = await _catalogRepository.FindAsync(catalog.ParentId);
            if (parent is null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "error.parantCatalogNotFound",
                    Description = "Parent catalog not found!"
                });
            }
            catalog.NormalizedName = $"{parent.NormalizedName}/{catalog.NormalizedName}";
        }
        catalog.CreatedDate = DateTime.Now;
        catalog.ModifiedDate = DateTime.Now;
        catalog.CreatedBy = _hcaService.GetUserId();
        catalog.ViewCount = 0;
        if (string.IsNullOrEmpty(catalog.Locale))
        {
            catalog.Locale = _options.DefaultLanguage;
        }
        await _catalogRepository.AddAsync(catalog);
        return IdentityResult.Success;
    }

    public async Task<Catalog> EnsureDataAsync(string name, string locale, CatalogType type = CatalogType.Default)
    {
        var catalog = await _context.Catalogs.FirstOrDefaultAsync(x => x.NormalizedName.Equals(name.ToLower()) && x.Locale == locale);
        if (catalog is null)
        {
            if (!Languages.Codes.Contains(locale))
            {
                locale = "vi-VN";  
            }
            catalog = new Catalog
            {
                Name = name,
                NormalizedName = name,
                Active = true,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Type = type,
                ViewCount = 0,
                Locale = locale
            };
            await _context.Catalogs.AddAsync(catalog);
        }
        catalog.ViewCount++;
        await _catalogRepository.SaveChangesAsync();
        return catalog;
    }

    public async Task<Catalog?> GetByNameAsync(string? normalizedName)
    {
        var catalog = await _catalogRepository.FindByNameAsync(normalizedName);
        if (catalog is null) return default;

        catalog.ViewCount++;
        await _catalogRepository.SaveChangesAsync();
        return catalog;
    }

    public async Task<List<ComponentListItem>> ListComponentAsync(Guid catalogId)
    {
        var query = from a in _context.WorkContents
                    join b in _context.WorkItems on a.Id equals b.WorkId
                    join c in _context.Components on a.ComponentId equals c.Id
                    where b.CatalogId == catalogId && a.Active
                    orderby b.SortOrder ascending
                    select new ComponentListItem
                    {
                        Name = c.NormalizedName,
                        Id = a.Id
                    };
        return await query.ToListAsync();
    }

    public async Task<IdentityResult> UpdateThumbnailAsync(Catalog args)
    {
        var catalog = await _catalogRepository.FindAsync(args.Id);
        if (catalog is null)
        {
            return IdentityResult.Failed();
        }
        catalog.Thumbnail = args.Thumbnail;
        catalog.ModifiedDate = DateTime.Now;
        await _catalogRepository.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<ListResult<Catalog>> ArticleListAsync(ArticleFilterOptions filterOptions)
    {
        var searchTerm = SeoHelper.ToSeoFriendly(filterOptions.Name);
        var query = _context.Catalogs.Where(x => (string.IsNullOrEmpty(searchTerm) || x.NormalizedName.Contains(searchTerm))
        && x.Type == CatalogType.Article && x.Active).OrderByDescending(x => x.ModifiedDate);
        return await ListResult<Catalog>.Success(query, filterOptions);
    }

    public async Task<Catalog?> FindAsync(Guid id)
    {
        var catalog = await _catalogRepository.FindAsync(id);
        if (catalog is null) return default;
        catalog.ViewCount++;
        await _catalogRepository.UpdateAsync(catalog);
        await _catalogRepository.SaveChangesAsync();
        return catalog;
    }

    public async Task<IdentityResult> SaveAsync(Catalog args)
    {
        var catalog = await _catalogRepository.FindAsync(args.Id);
        if (catalog is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "catalog.notFound",
                Description = "Data not found!"
            });
        }
        var normalizedName = args.NormalizedName;
        if (string.IsNullOrWhiteSpace(args.NormalizedName))
        {
            normalizedName = SeoHelper.ToWikiFriendly(args.Name);
        }
        if (args.ParentId != null)
        {
            var parent = await _catalogRepository.FindAsync(args.ParentId);
            if (parent is null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "error.parentCatalogNotFound",
                    Description = "Parent catalog not found."
                });
            }
            catalog.ParentId = parent.Id;
            if (!normalizedName.Contains('/'))
            {
                normalizedName = $"{parent.NormalizedName}/{normalizedName}";
            }
            else
            {
                var nameDeserlizer = normalizedName.Split('/');
                if (!nameDeserlizer.First().Equals(parent.NormalizedName))
                {
                    normalizedName = $"{parent.NormalizedName}/{nameDeserlizer.Last()}";
                }
            }
        }

        catalog.Name = args.Name;
        if (catalog.Type != CatalogType.Entry)
        {
            catalog.NormalizedName = normalizedName;
        }
        catalog.Active = args.Active;
        catalog.ModifiedDate = DateTime.Now;
        catalog.Description = args.Description;
        catalog.Thumbnail = args.Thumbnail;
        if (!string.IsNullOrWhiteSpace(args.Locale))
        {
            catalog.Locale = args.Locale;
        }
        catalog.Type = args.Type;
        await _catalogRepository.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public IEnumerable<Option> GetTypes()
    {
        var values = Enum.GetValues(typeof(CatalogType));
        foreach (var item in values)
        {
            yield return new Option
            {
                Value = item.GetHashCode().ToString(),
                Label = item.ToString()
            };
        }
    }

    public async Task<ListResult<Catalog>> ListAsync(CatalogFilterOptions filterOptions)
    {
        filterOptions.Name = SeoHelper.ToSeoFriendly(filterOptions.Name);
        return await _catalogRepository.ListAsync(filterOptions);
    }

    public async Task<ListResult<Catalog>?> ArticleRelatedListAsync(ArticleRelatedFilterOption filterOption)
    {
        if (!filterOption.TagIds.Any()) return default;
        var query = (from tag in _context.WorkItems
                     join catalog in _context.Catalogs on tag.WorkId equals catalog.Id
                     where catalog.Active && filterOption.TagIds.Contains(tag.CatalogId) && catalog.Type == filterOption.Type && catalog.Id != filterOption.CatalogId
                     select catalog).Distinct().OrderByDescending(x => x.ModifiedDate);
        return await ListResult<Catalog>.Success(query, filterOption);
    }

    public async Task<List<Catalog>> ListTagByIdAsync(Guid catalogId)
    {
        var query = from a in _context.WorkItems
                    join b in _context.Catalogs on a.CatalogId equals b.Id
                    where b.Active && a.WorkId == catalogId && b.Type == CatalogType.Tag
                    orderby a.SortOrder ascending
                    select b;
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Option>> ListTagSelectAsync(TagFilterOptions filterOptions)
    {
        if (string.IsNullOrWhiteSpace(filterOptions.KeyWords)) return new List<Option>();

        var normalizedName = SeoHelper.ToSeoFriendly(filterOptions.KeyWords);

        return await _context.Catalogs.Where(x =>
        x.Active && x.Type == CatalogType.Tag && x.NormalizedName.Contains(normalizedName))
            .Select(x => new Option
            {
                Label = x.Name,
                Value = x.Id
            }).Take(20).ToListAsync();
    }

    public async Task<IdentityResult> TagAddToCatalogAsync(WorkItem args)
    {
        var tag = await _catalogRepository.FindAsync(args.CatalogId);
        if (tag is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Tag not found!"
            });
        }
        var catalog = await _catalogRepository.FindAsync(args.WorkId);
        if (catalog is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "Catalog not found!"
            });
        }
        await _context.WorkItems.AddAsync(args);
        await _catalogRepository.SaveChangesAsync();
        return IdentityResult.Success;
    }

    public async Task<ListResult<Catalog>> ListByTagsAsync(IEnumerable<Guid> tagIds, CatalogFilterOptions filterOptions)
    {
        var searchTerm = SeoHelper.ToSeoFriendly(filterOptions.Name);
        var query = from a in _context.WorkItems
                    join b in _context.Catalogs on a.WorkId equals b.Id
                    where tagIds.Contains(a.CatalogId) && b.Active &&
                    (string.IsNullOrEmpty(searchTerm) || b.NormalizedName.Contains(searchTerm)) &&
                    (filterOptions.Type == null || b.Type == filterOptions.Type)
                    orderby b.ModifiedDate descending
                    select b;
        return await ListResult<Catalog>.Success(query, filterOptions);
    }

    public async Task<ListResult<Catalog>> ListByTagAsync(Guid tagId, CatalogFilterOptions filterOptions)
    {
        var searchTerm = SeoHelper.ToSeoFriendly(filterOptions.Name);
        var query = from a in _context.WorkItems
                    join b in _context.Catalogs on a.WorkId equals b.Id
                    where a.CatalogId == tagId && b.Active &&
                    (string.IsNullOrEmpty(searchTerm) || b.NormalizedName.Contains(searchTerm)) &&
                    (filterOptions.Type == null || b.Type == filterOptions.Type)
                    orderby b.ModifiedDate descending
                    select b;
        return await ListResult<Catalog>.Success(query, filterOptions);
    }

    public async Task<IEnumerable<Catalog>> ListRandomTagAsync() => await _context.Catalogs.Where(x => x.Type == CatalogType.Tag && x.Active).OrderBy(x => Guid.NewGuid()).Take(10).ToListAsync();

    public async Task<ListResult<TagListItem>> ListTagAsync(TagFilterOptions filterOptions)
    {
        var searchTerm = SeoHelper.ToSeoFriendly(filterOptions.KeyWords);
        var query = _context.Catalogs
            .Where(x => x.Type == CatalogType.Tag && x.Active)
            .Where(x => string.IsNullOrEmpty(searchTerm) || x.NormalizedName.Contains(searchTerm))
            .Select(x => new TagListItem
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                ViewCount = x.ViewCount,
                PostCount = _context.WorkItems.Count(y => y.CatalogId == x.Id)
            }).OrderByDescending(x => x.Id);
        if (filterOptions.OrderBy != null)
        {
            if (filterOptions.OrderBy == Constants.OrderBy.View)
            {
                query = query.OrderByDescending(x => x.ViewCount);
            }
        }
        return await ListResult<TagListItem>.Success(query, filterOptions);
    }

    public async Task<object?> PieChartAsync()
    {
        return await _context.Catalogs.GroupBy(x => x.Type).Select(x => new
        {
            label = x.Key.ToString(),
            value = x.Count()
        }).ToListAsync();
    }

    public async Task<ProductImage?> GetProductImageAsync(Guid catalogId)
    {
        var component = await _componentRepository.FindByNameAsync(nameof(ProductImage));
        if (component is null) return default;
        var work = await _workRepository.FindByCatalogAsync(catalogId, component.Id);
        if (work is null || string.IsNullOrEmpty(work.Arguments)) return default;
        return JsonSerializer.Deserialize<ProductImage?>(work.Arguments);
    }

    public async Task<IEnumerable<Option>> GetFormSelectAsync(SelectFilterOptions filterOptions)
    {
        filterOptions.KeyWords = SeoHelper.ToSeoFriendly(filterOptions.KeyWords);
        return await _catalogRepository.GetFormSelectAsync(filterOptions);
    }

    public Task<IEnumerable<Catalog>> ListSpotlightAsync(CatalogType type, int pageSize) => _catalogRepository.ListSpotlightAsync(type, pageSize);

    public async Task DeleteAsync(Catalog catalog) => await _catalogRepository.DeleteAsync(catalog);

    public Task<Catalog?> FindAsync(Guid catalogId, CatalogType type) => _catalogRepository.FindAsync(catalogId, type);

    public Task<IEnumerable<Catalog>> GetTopViewAsync(CatalogType type) => _catalogRepository.GetTopViewAsync(type);

    public Task<IEnumerable<Guid>> ListTagIdsAsync(Guid id) => _workRepository.ListTagIdsAsync(id);

    public Task<object?> GetStructureByIdAsync(Guid id) => _catalogRepository.GetStructureAsync(id);

    public Task<int> GetViewCountAsync() => _catalogRepository.GetViewCountAsync();

    public async Task<object?> GetComponentsAsync(GetComponentsArgs args)
    {
        var catalog = await _catalogRepository.FindByNameAsync(args.NormalizedName);
        if (catalog is null) return default;
        return await _catalogRepository.GetComponentsAsync(catalog.Id);
    }

    public Task<object?> GetStructureAsync(string normalizedName) => _catalogRepository.GetStructureAsync(normalizedName);
}
