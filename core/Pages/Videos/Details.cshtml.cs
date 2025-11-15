using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Pages.Videos;

public class DetailsModel : DynamicPageModel
{
    public List<ComponentListItem> Components = new();

    public DetailsModel(ICatalogService catalogService) : base(catalogService)
    {
    }

    public List<Catalog> Tags = new();
    public bool HasTag => Tags.Any();

    public async Task<IActionResult> OnGetAsync()
    {
        Components = await _catalogService.ListComponentAsync(PageData.Id);
        Tags = await _catalogService.ListTagByIdAsync(PageData.Id);
        return Page();
    }
}
