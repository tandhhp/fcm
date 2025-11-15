using Microsoft.AspNetCore.Mvc.RazorPages;
using Waffle.Core.Constants;

namespace Waffle.Pages.Users;

public class LogoutModel : PageModel
{
    public void OnGet()
    {
        Response.Cookies.Delete(CookieKey.Token);
    }
}
