using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;

namespace Waffle.Core.Foundations
{
    public abstract class DynamicPageModel : PageModel
    {
        protected readonly ICatalogService _catalogService;
        public DynamicPageModel(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public Catalog PageData { protected set; get; } = new();
        public Catalog? Category { private set; get; }

        public override async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            context.RouteData.Values.TryGetValue("category", out var category);
            context.RouteData.Values.TryGetValue("normalizedName", out var normalizedName);
            if (!string.IsNullOrEmpty(category?.ToString()))
            {
                Category = await _catalogService.GetByNameAsync(category?.ToString());
                RouteData.Values.TryAdd("Parent", Category);
                if (Category != null && CatalogType.WordPress == Category.Type)
                {
                    PageData = new Catalog
                    {
                        NormalizedName = normalizedName?.ToString() ?? string.Empty,
                        Type = CatalogType.WordPress
                    };
                    RouteData.Values.TryAdd(nameof(Catalog), PageData);
                    return;
                }
                normalizedName = $"{category}/{normalizedName}";
            }
            var catalog = await _catalogService.GetByNameAsync(normalizedName?.ToString());
            if (catalog is null)
            {
                context.HttpContext.Response.StatusCode = 404;
                context.HttpContext.Response.Redirect("/exception/notfound");
                return;
            }
            PageData = catalog;
            ViewData["Title"] = catalog.Name;
            ViewData["Description"] = catalog.Description;
            ViewData["Image"] = catalog.Thumbnail;
            RouteData.Values.TryAdd(nameof(Catalog), PageData);
        }
    }
}
