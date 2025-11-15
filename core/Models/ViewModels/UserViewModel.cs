using Waffle.Core.Helpers;

namespace Waffle.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Avatar => $"https://www.gravatar.com/avatar/{EncryptHelper.MD5Create(Email)}?s=520";
    }
}
