using Waffle.Models;

namespace Waffle.Core.Services.Users.Models;

public class TelesalesSelectOptions : SelectOptions
{
    public Guid? TelesalesManagerId { get; set; }
}
