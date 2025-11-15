using Waffle.Entities;

namespace Waffle.Models.ViewModels.Users;

public class CurrentUserViewModel : ApplicationUser
{
    public IList<string> Roles { get; set; } = [];
}
