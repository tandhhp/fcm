using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Pages.Articles;

public class IndexModel : EntryPageModel
{

    [BindProperty(SupportsGet = true)]
    public int Current { get; set; } = 1;

    public ListResult<Catalog>? Articles;

    public IndexModel(ICatalogService catalogService) : base(catalogService)
    {
    }

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public IActionResult OnGet()
    {
        return Redirect("https://nuras.vn");
    }
}
