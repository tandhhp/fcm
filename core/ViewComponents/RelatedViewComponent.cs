using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class RelatedViewComponent : ViewComponent
{
    private readonly ICatalogService _catalogService;
    private readonly IProductService _productService;

    public RelatedViewComponent(ICatalogService catalogService, IProductService productService)
    {
        _catalogService = catalogService;
        _productService = productService;
    }

    private Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    public async Task<IViewComponentResult> InvokeAsync(IEnumerable<Guid> tagIds)
    {
        if (!tagIds.Any())
        {
            return View(Empty.DefaultView, new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        if (PageData.Type == CatalogType.Product)
        {
            var products = await _productService.ListRelatedAsync(tagIds, PageData.Id);
            return View("Product", products);
        }
        var articles = await _catalogService.ArticleRelatedListAsync(new ArticleRelatedFilterOption
        {
            Current = 1,
            PageSize = 4,
            CatalogId = PageData.Id,
            TagIds = tagIds,
            Type = PageData.Type
        });
        if (articles?.Data == null || !articles.Data.Any())
        {
            return View(Empty.DefaultView, new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        return View(articles.Data);
    }
}
