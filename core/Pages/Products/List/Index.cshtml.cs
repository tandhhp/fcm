using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models;

namespace Waffle.Pages.Products.List;

public class IndexModel : EntryPageModel
{
    public IndexModel(ICatalogService catalogService) : base(catalogService)
    {
    }

    public IEnumerable<ComponentListItem> Components = new List<ComponentListItem>();
    [BindProperty(SupportsGet = true)]
    [UIHint(UIHint.SearchBox)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync()
    {
        Components = await _catalogService.ListComponentAsync(PageData.Id);
    }
}
