using Waffle.Entities;
using Waffle.Models.Components;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Foundations;
using Waffle.ExternalAPI.Models;

namespace Waffle.ViewComponents.ECommerces;

public class ShopeeProductViewComponent : BaseViewComponent<ShopeeProduct>
{
    private readonly IShopeeService _shopeeService;
    public ShopeeProductViewComponent(IShopeeService shopeeService, IWorkService workService): base (workService)
    {
        _shopeeService = shopeeService;
    }

    private static IEnumerable<Catalog> MapProduct(List<LinkList> linkLists)
    {
        foreach (var item in linkLists)
        {
            yield return new Catalog
            {
                Name = item.LinkName ?? string.Empty,
                Thumbnail = item.Image,
                NormalizedName = item.Link ?? string.Empty
            };
        }
    }

    protected override async Task<ShopeeProduct> ExtendAsync(ShopeeProduct work)
    {
        var response = await _shopeeService.GetLinkListAsync(work.UrlSuffix, work.GroupId, string.Empty);
        if (response != null)
        {
            work.Products = MapProduct(response.LinkList);
        }
        return work;
    }
}
