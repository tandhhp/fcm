using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Pages.Products
{
    public class IndexModel : EntryPageModel
    {
        private readonly IShopeeService _shopeeService;

        public IndexModel(ICatalogService catalogService, IShopeeService shopeeService) : base(catalogService)
        {
            _shopeeService = shopeeService;
        }

        public ListResult<Catalog>? Categories;
        public BaseInfoAndLinks BaseInfoAndLinks = new();
        public List<ComponentListItem> Components = new();

        [BindProperty(SupportsGet = true)]
        public int Current { get; set; } = 1;
        public Pagination Pagination => new()
        {
            NextPageUrl = $"/shop?current={Current + 1}",
            PrevPageUrl = $"/shop?current={Current - 1}",
        };

        public async Task<IActionResult> OnGetAsync()
        {
            BaseInfoAndLinks = await _shopeeService.GetBaseInfoAndLinksAsync(Current);

            Components = await _catalogService.ListComponentAsync(PageData.Id);

            return Page();
        }
    }
}
