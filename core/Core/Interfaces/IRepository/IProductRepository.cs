using Microsoft.AspNetCore.Identity;
using Waffle.Entities.Ecommerces;
using Waffle.Models;
using Waffle.Models.Params.Products;
using Waffle.Models.ViewModels.Products;

namespace Waffle.Core.Interfaces.IRepository;

public interface IProductRepository : IAsyncRepository<Product>
{
    Task<Product?> FindByCatalogAsync(Guid catalogId);
    Task<ListResult<ProductListItem>> ListAsync(ProductFilterOptions filterOptions);
    Task<IEnumerable<ProductListItem>> ListByTagAsync(Guid tagId, CatalogFilterOptions filterOptions);
    Task<IEnumerable<ProductListItem>> ListRelatedAsync(IEnumerable<Guid> tagIds, Guid currentCatalogId);
    Task<IEnumerable<ProductListItem>> ListSpotlightAsync(int pageSize, IEnumerable<Guid> tagIds);
    Task<IdentityResult> SaveBrandAsync(SaveBrandModel args);
}
