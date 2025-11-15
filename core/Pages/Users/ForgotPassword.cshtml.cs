using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Waffle.Entities;

namespace Waffle.Pages.Users;

public class ForgotPasswordModel : Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.ForgotPasswordModel
{
    private readonly UserManager<ApplicationUser> _userManager; 
    private readonly IEmailSender _emailSender;
    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public string Error { get; set; } = string.Empty;

    public override async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user == null)
        {
            Error = "User not found!";
            return Page();
        }

        if (!user.EmailConfirmed)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var confirmCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmUrl = Url.Page(
                "/User/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code = confirmCode },
            protocol: Request.Scheme) ?? "/Users/ConfirmEmail";

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmUrl)}'>clicking here</a>.");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Page(
            "/User/ResetPassword",
            pageHandler: null,
            values: new { code },
            protocol: Request.Scheme) ?? "/";

        await _emailSender.SendEmailAsync(
            Input.Email,
            "Reset Password",
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return RedirectToPage("./ForgotPasswordConfirmation");
    }
}
