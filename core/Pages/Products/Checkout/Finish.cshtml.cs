using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models;

namespace Waffle.Pages.Products.Checkout;

public class FinishModel : EntryPageModel
{
    public FinishModel(ICatalogService catalogService) : base(catalogService)
    {
    }
    public List<ComponentListItem> Components { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Components = await _catalogService.ListComponentAsync(PageData.Id);
        return Page();
    }
}
