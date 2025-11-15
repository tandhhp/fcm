using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.ECommerces;

public class ProductImageViewComponent : ViewComponent
{
    private readonly ICatalogService _catalogService;
    private readonly IWorkService _workService;

    public ProductImageViewComponent(IWorkService workService, ICatalogService catalogService)
    {
        _workService = workService;
        _catalogService = catalogService;
    }

    private Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid workId)
    {
        var work = await _catalogService.GetProductImageAsync(PageData.Id);
        work ??= new ProductImage();
        if (!work.Images.Any(x => x.Equals(PageData.Thumbnail)))
        {
            work.Images.Insert(0, PageData.Thumbnail ?? "/imgs/search-engines-amico.svg");
        }
        return View(work);
    }
}
