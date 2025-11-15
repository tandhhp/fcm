using Microsoft.AspNetCore.Identity;

namespace Waffle.Core.Interfaces
{
    public interface IEmailSender
    {
        Task<IdentityResult> SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
