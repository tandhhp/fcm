using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Services;

namespace Waffle.ViewComponents.ECommerces;

public class ShopeeSpotlightViewComponent : ViewComponent
{
    private readonly IShopeeService _shopeeService;

    public ShopeeSpotlightViewComponent(IShopeeService shopeeService)
    {
        _shopeeService = shopeeService;
    }
    protected Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var shopee = await _shopeeService.GetLinkListsAsync(PageData.Name, 1, 4);
        shopee.KeyWord = PageData.Name;
        return View("/Pages/Components/Products/ShopeeSpotlight/Default.cshtml", shopee);
    }
}
