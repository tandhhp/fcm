using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Ecommerces;
using Waffle.Extensions;
using Waffle.Models;
using Waffle.Models.Params.Products;
using Waffle.Models.ViewModels.Products;

namespace Waffle.Infrastructure.Repositories;

public class ProductRepository : EfRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Product?> FindByCatalogAsync(Guid catalogId) => await _context.Products.FirstOrDefaultAsync(x => x.CatalogId == catalogId);

    public async Task<ListResult<ProductListItem>> ListAsync(ProductFilterOptions filterOptions)
    {
        var query = from catalog in _context.Catalogs
                    join product in _context.Products on catalog.Id equals product.CatalogId into productCatalog from product in productCatalog.DefaultIfEmpty()
                    where catalog.Active && catalog.Type == CatalogType.Product 
                    && (string.IsNullOrEmpty(filterOptions.Name) || catalog.NormalizedName.Contains(filterOptions.Name))
                    && (filterOptions.ParentId == null || catalog.ParentId == filterOptions.ParentId)
                    && (filterOptions.Active == null || catalog.Active == filterOptions.Active)
                    && catalog.Locale == filterOptions.Locale
                    orderby catalog.ModifiedDate descending
                    select new ProductListItem
                    {
                        Id = catalog.Id,
                        Name = catalog.Name,
                        NormalizedName = catalog.NormalizedName,
                        Url = catalog.GetUrl(),
                        Thumbnail = catalog.Thumbnail,
                        ViewCount = catalog.ViewCount,
                        Price = product.Point,
                        SalePrice = product.Point
                    };
        return await ListResult<ProductListItem>.Success(query, filterOptions);
    }

    public async Task<IEnumerable<ProductListItem>> ListByTagAsync(Guid tagId, CatalogFilterOptions filterOptions)
    {
        var query = from a in _context.WorkItems
                    join b in _context.Catalogs on a.WorkId equals b.Id
                    join product in _context.Products on b.Id equals product.CatalogId into productCatalog from product in productCatalog.DefaultIfEmpty()
                    where a.CatalogId == tagId && b.Active &&
                    (string.IsNullOrEmpty(filterOptions.Name) || b.NormalizedName.Contains(filterOptions.Name)) &&
                    (filterOptions.Type == null || b.Type == filterOptions.Type)
                    orderby b.ModifiedDate descending
                    select new ProductListItem
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Url = b.GetUrl(),
                        Thumbnail = b.Thumbnail,
                        Price= product.Point,
                        SalePrice = product.Point
                    };
        return await query.OrderBy(x => Guid.NewGuid()).Take(4).ToListAsync();
    }

    public async Task<IEnumerable<ProductListItem>> ListRelatedAsync(IEnumerable<Guid> tagIds, Guid currentCatalogId)
    {
        var query = (from tag in _context.WorkItems
                     join catalog in _context.Catalogs on tag.WorkId equals catalog.Id
                     join product in _context.Products on catalog.Id equals product.CatalogId into productCatalog from product in productCatalog.DefaultIfEmpty()
                     where catalog.Active && tagIds.Contains(tag.CatalogId) && catalog.Type == CatalogType.Product && catalog.Id != currentCatalogId
                     select new ProductListItem
                     {
                         Id = catalog.Id,
                         Name = catalog.Name,
                         Url = catalog.GetUrl(),
                         Thumbnail = catalog.Thumbnail,
                         Price = product.Point,
                         SalePrice = product.Point
                     }).Distinct().OrderByDescending(x => Guid.NewGuid());
        return await query.Take(4).ToListAsync();
    }

    public async Task<IEnumerable<ProductListItem>> ListSpotlightAsync(int pageSize, IEnumerable<Guid> tagIds)
    {
        var query = from catalog in _context.Catalogs
                    join product in _context.Products on catalog.Id equals product.CatalogId into catalogProduct
                    from product in catalogProduct.DefaultIfEmpty()
                    join tag in _context.WorkItems on catalog.Id equals tag.WorkId
                    where catalog.Type == CatalogType.Product && catalog.Active
                    && (!tagIds.Any() || tagIds.Contains(tag.CatalogId))
                    select new ProductListItem
                    {
                        Name = catalog.Name,
                        Id = catalog.Id,
                        Url = catalog.GetUrl(),
                        Price = product.Point,
                        SalePrice = product.Point,
                        Thumbnail = catalog.Thumbnail,
                        ViewCount = catalog.ViewCount,
                        ModifiedDate = catalog.ModifiedDate
                    };
        return await query.Distinct().OrderBy(x => Guid.NewGuid()).Take(pageSize).AsNoTracking().ToListAsync();
    }

    public async Task<IdentityResult> SaveBrandAsync(SaveBrandModel args)
    {
        var product = await _context.Catalogs.FindAsync(args.ProductId);
        if (product is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.dataNotFound",
                Description = "Product not found!"
            });
        }
        var brand = await _context.Catalogs.FindAsync(args.BrandId);
        if (brand is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.dataNotFound",
                Description = "Brand not found!"
            });
        }
        if (!product.NormalizedName.Contains('/'))
        {
            product.NormalizedName = $"{brand.NormalizedName}/{product.NormalizedName}";
        }
        product.ParentId = args.BrandId;
        await _context.SaveChangesAsync();
        return IdentityResult.Success;
    }
}
