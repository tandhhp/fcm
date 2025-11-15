using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;

namespace Waffle.Pages.Products.Shopee;

public class IndexModel : EntryPageModel
{
    private readonly IShopeeService _shopeeService;
    public IndexModel(ICatalogService catalogService, IShopeeService shopeeService) : base(catalogService)
    {
        _shopeeService = shopeeService;
    }

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)]
    public int PageNum { get; set; } = 1;

    public LandingPageLinkList Product = new();

    public async Task OnGetAsync()
    {
        Product = await _shopeeService.GetLinkListsAsync(SearchTerm, PageNum, 12);
    }
}
