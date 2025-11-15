using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.ECommerces;

public class ProductSpotlightViewComponent : ViewComponent
{
    private readonly IProductService _productService;
    private readonly IWorkService _workService;
    private readonly ICatalogService _catalogService;

    protected Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    public ProductSpotlightViewComponent(IProductService productService, IWorkService workService, ICatalogService catalogService)
    {
        _productService = productService;
        _workService = workService;
        _catalogService = catalogService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid? workId)
    {
        var work = await _workService.GetAsync<ProductSpotlight>(workId);
        var tagIds = await _catalogService.ListTagIdsAsync(PageData.Id);
        if (work is null)
        {
            work ??= new ProductSpotlight();
            work.Products = await _productService.ListSpotlightAsync(4, tagIds);
        }
        else
        {
            work.Products = await _productService.ListSpotlightAsync(work.PageSize, tagIds);
        }
        return View(work);
    }
}
