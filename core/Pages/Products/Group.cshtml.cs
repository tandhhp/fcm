using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Models.Components;

namespace Waffle.Pages.Products
{
    public class GroupModel : PageModel
    {
        private readonly IShopeeService _shopeeService;
        public GroupModel(IShopeeService shopeeService)
        {
            _shopeeService = shopeeService;
        }

        public LandingPageLinkList Data = new();
        public string GroupId = string.Empty;
        [BindProperty(SupportsGet = true)]
        public int Current { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;
        public Pagination Pagination => new()
        {
            NextPageUrl = $"/shop/group/{GroupId}?current={Current + 1}",
            PrevPageUrl = $"/shop/group/{GroupId}?current={Current - 1}",
        };

        public async Task<IActionResult> OnGetAsync(string groupId)
        {
            if (string.IsNullOrEmpty(groupId)) {
                return NotFound();
            }
            ViewData["Title"] = string.IsNullOrEmpty(SearchTerm) ? groupId : SearchTerm;
            GroupId = groupId;
            Data = await _shopeeService.GetLinkListAsync("banhque", groupId, SearchTerm);
            return Page();
        }
    }
}
