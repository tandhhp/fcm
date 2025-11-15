using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Waffle.Entities;

namespace Waffle.Pages.Users;

public class EditModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    public EditModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;    
    }

    [BindProperty(SupportsGet = true)]
    public string? PhoneNumber { get; set; }
    [BindProperty(SupportsGet = true)]
    public string Name { get; set; } = default!;
    [BindProperty(SupportsGet = true)]
    public string? Address { get; set; }
    [BindProperty(SupportsGet = true)]
    public string UserId { get; set; } = default!;

    public ApplicationUser? CurrentUser;
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser == null) return NotFound();
        ViewData["Title"] = CurrentUser.UserName;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user is null) return NotFound();
        user.Name = Name;
        user.Address = Address;
        user.PhoneNumber = PhoneNumber;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return Redirect($"/users/{user.Id}");
        return Page();
    }
}
