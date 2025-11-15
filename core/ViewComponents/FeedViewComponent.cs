using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class FeedViewComponent : BaseViewComponent<Feed>
{
    private readonly ICatalogService _catalogService;
    private readonly IProductService _productService;

    public FeedViewComponent(ICatalogService catalogService, IWorkService workService, IProductService productService) : base(workService)
    {
        _catalogService = catalogService;
        _productService = productService;
    }

    protected override async Task<Feed> ExtendAsync(Feed feed)
    {
        var type = feed.Type ?? CatalogType.Article;
        ViewName = type.ToString();
        if (type == CatalogType.Article)
        {
            var articles = await _catalogService.ListAsync(new CatalogFilterOptions
            {
                Active = true,
                PageSize = feed.PageSize,
                Type = type,
                Locale = PageData.Locale
            });
            feed.Articles = articles.Data?.ToList() ?? new();
            return feed;
        }
        var products = await _productService.ListAsync(new ProductFilterOptions
        {
            Current = 1,
            PageSize = feed.PageSize,
            Active = true
        });
        feed.Products = products.Data;
        return feed;
    }
}
