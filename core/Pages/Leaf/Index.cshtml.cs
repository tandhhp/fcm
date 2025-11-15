using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Pages.Leaf;

public class IndexModel : EntryPageModel
{

    public ListResult<Catalog> Catalogs = new();

    public IndexModel(ICatalogService catalogService) : base(catalogService)
    {
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Catalogs = await _catalogService.ListAsync(new()
        {
            Type = CatalogType.Default,
            Active = true,
            PageSize = 20
        });
        return Page();
    }
}
