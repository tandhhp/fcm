using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;

namespace Waffle.Pages.Products.Cart;

public class IndexModel : EntryPageModel
{
    public IndexModel(ICatalogService catalogService) : base(catalogService)
    {
    }

    public void OnGet()
    {
    }
}
