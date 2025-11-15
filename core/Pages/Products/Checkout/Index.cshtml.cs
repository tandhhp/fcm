using Microsoft.AspNetCore.Identity;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;

namespace Waffle.Pages.Products.Checkout;

public class IndexModel : EntryPageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(UserManager<ApplicationUser> userManager, ICatalogService catalogService) : base(catalogService)
    {
        _userManager = userManager;
    }

    public bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;

    public ApplicationUser? CurrentUser;

    public async Task OnGetAsync()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
    }
}
