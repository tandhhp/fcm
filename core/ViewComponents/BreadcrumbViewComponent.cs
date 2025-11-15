using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Options;
using Waffle.Entities;
using Waffle.Extensions;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class BreadcrumbViewComponent : ViewComponent
{
    private readonly ILocalizationService _localizationService;
    private readonly ICatalogService _catalogService;
    private readonly SettingOptions Options;
    public BreadcrumbViewComponent(ICatalogService catalogService, ILocalizationService localizationService, IOptions<SettingOptions> options)
    {
        _catalogService = catalogService;
        _localizationService = localizationService;
        Options = options.Value;
    }

    private Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    private Catalog? Category
    {
        get
        {
            RouteData.Values.TryGetValue("Parent", out var values);
            return values as Catalog;
        }
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var breadcrumb = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Url = "/",
                Name = await _localizationService.GetAsync("home"),
                Position = 1,
                Icon = "fas fa-home"
            }
        };

        if (Category != null)
        {
            breadcrumb.Add(new Breadcrumb
            {
                Url = $"/leaf/{Category.NormalizedName}",
                Name = Category.Name,
                Position = breadcrumb.Count + 1
            });
        }

        breadcrumb.Add(new Breadcrumb
        {
            Url = PageData.GetUrl(),
            Name = PageData.Name,
            Position = breadcrumb.Count + 1
        });
        return View(Options.Theme, breadcrumb);
    }
}
