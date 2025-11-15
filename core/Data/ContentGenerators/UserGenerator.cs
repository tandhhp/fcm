using Microsoft.AspNetCore.Identity;
using Waffle.Core.Foundations;
using Waffle.Entities;

namespace Waffle.Data.ContentGenerators
{
    public class UserGenerator : BaseGenerator
    {
        public UserGenerator(ApplicationDbContext context) : base(context)
        {
        }

        public async Task EnsureUsersAsync()
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@defzone.net"
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = passwordHasher.HashPassword(user, "Password@123");
            user.PasswordHash = hashedPassword;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public override Task RunAsync()
        {
            throw new NotImplementedException();
        }
    }
}
