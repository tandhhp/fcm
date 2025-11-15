using Microsoft.AspNetCore.Identity;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Args.Catalogs;
using Waffle.Models.Components;
using Waffle.Models.ViewModels;
using Waffle.Models.ViewModels.Products;

namespace Waffle.Core.Interfaces.IService;

public interface ICatalogService
{
    Task<IdentityResult> ActiveAsync(Guid id);
    Task<IdentityResult> AddAsync(Catalog catalog);
    Task<Catalog> EnsureDataAsync(string name, string locale, CatalogType type = CatalogType.Default);
    Task<Catalog?> GetByNameAsync(string? normalizedName);
    Task<List<ComponentListItem>> ListComponentAsync(Guid catalogId);
    Task<IdentityResult> UpdateThumbnailAsync(Catalog args);
    Task<ListResult<Catalog>> ArticleListAsync(ArticleFilterOptions filterOptions);
    Task<ListResult<Catalog>?> ArticleRelatedListAsync(ArticleRelatedFilterOption filterOptions);
    Task<Catalog?> FindAsync(Guid id);
    Task<IdentityResult> SaveAsync(Catalog args);
    IEnumerable<Option> GetTypes();
    Task<ListResult<Catalog>> ListAsync(CatalogFilterOptions filterOptions);
    Task<List<Catalog>> ListTagByIdAsync(Guid id);
    Task<IEnumerable<Option>> ListTagSelectAsync(TagFilterOptions filterOptions);
    Task<IdentityResult> TagAddToCatalogAsync(WorkItem args);
    Task<ListResult<Catalog>> ListByTagsAsync(IEnumerable<Guid> tagIds, CatalogFilterOptions filterOptions);
    Task<ListResult<Catalog>> ListByTagAsync(Guid tagId, CatalogFilterOptions filterOptions);
    Task<IEnumerable<Catalog>> ListRandomTagAsync();
    Task<ListResult<TagListItem>> ListTagAsync(TagFilterOptions filterOptions);
    Task<object?> PieChartAsync();
    Task<ProductImage?> GetProductImageAsync(Guid catalogId);
    Task<IEnumerable<Option>> GetFormSelectAsync(SelectFilterOptions filterOptions);
    Task<IEnumerable<Catalog>> ListSpotlightAsync(CatalogType type, int pageSize);
    Task DeleteAsync(Catalog catalog);
    Task<Catalog?> FindAsync(Guid brandId, CatalogType brand);
    Task<IEnumerable<Catalog>> GetTopViewAsync(CatalogType type);
    Task<IEnumerable<Guid>> ListTagIdsAsync(Guid id);
    Task<object?> GetStructureByIdAsync(Guid id);
    Task<int> GetViewCountAsync();
    Task<object?> GetComponentsAsync(GetComponentsArgs args);
    Task<object?> GetStructureAsync(string normalizedName);
}
