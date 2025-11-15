using Microsoft.AspNetCore.Identity;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Entities.Ecommerces;
using Waffle.Models;
using Waffle.Models.Params.Products;
using Waffle.Models.ViewModels.Products;

namespace Waffle.Core.Services.Ecommerces;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICatalogRepository _catalogRepository;

    public ProductService(IProductRepository productRepository, ICatalogRepository catalogRepository)
    {
        _productRepository = productRepository;
        _catalogRepository = catalogRepository;
    }

    public Task<int> CountAsync() => _catalogRepository.CountAsync(CatalogType.Product);

    public async Task<Product?> GetByCatalogIdAsync(Guid catalogId) => await _productRepository.FindByCatalogAsync(catalogId);

    public Task<ListResult<ProductListItem>> ListAsync(ProductFilterOptions filterOptions)
    {
        filterOptions.Name = SeoHelper.ToSeoFriendly(filterOptions.Name);
        return _productRepository.ListAsync(filterOptions);
    }

    public Task<IEnumerable<ProductListItem>> ListByTagAsync(Guid tagId, CatalogFilterOptions filterOptions)
    {
        filterOptions.Name = SeoHelper.ToSeoFriendly(filterOptions.Name);
        return _productRepository.ListByTagAsync(tagId, filterOptions);
    }

    public Task<IEnumerable<ProductListItem>> ListRelatedAsync(IEnumerable<Guid> tagIds, Guid currentCatalogId) => _productRepository.ListRelatedAsync(tagIds, currentCatalogId);

    public Task<IEnumerable<ProductListItem>> ListSpotlightAsync(int pageSize, IEnumerable<Guid> tagIds) => _productRepository.ListSpotlightAsync(pageSize, tagIds);

    public async Task<IdentityResult> SaveAsync(Product args)
    {
        var product = await _productRepository.FindByCatalogAsync(args.CatalogId);
        if (product is null)
        {
            product = new Product
            {
                CatalogId = args.CatalogId,
                SKU = args.SKU,
                UnitInStock = args.UnitInStock,
                Content = args.Content,
                Brand = args.Brand,
                Point = args.Point,
                Galleries = args.Galleries,
                Summary = args.Summary,
                Cert1File = args.Cert1File,
                Cert1Name = args.Cert1Name,
                Cert2File = args.Cert2File,
                Cert2Name = args.Cert2Name,
            };
            await _productRepository.AddAsync(product);

        }
        else
        {
            product.SKU = args.SKU;
            product.UnitInStock = args.UnitInStock;
            product.Point = args.Point;
            product.Brand = args.Brand;
            product.Content = args.Content;
            product.Galleries = args.Galleries;
            product.Summary = args.Summary;
            product.Cert1Name = args.Cert1Name;
            product.Cert1File = args.Cert1File;
            product.Cert2File = args.Cert2File;
            product.Cert2Name = args.Cert2Name;
            await _productRepository.SaveChangesAsync();
        }
        return IdentityResult.Success;
    }

    public Task<IdentityResult> SaveBrandAsync(SaveBrandModel args) => _productRepository.SaveBrandAsync(args);
}
