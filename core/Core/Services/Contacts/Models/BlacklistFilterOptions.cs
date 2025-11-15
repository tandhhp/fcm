using Waffle.Models;

namespace Waffle.Core.Services.Contacts.Models;

public class BlacklistFilterOptions : FilterOptions
{
    public string? PhoneNumber { get; set; }
    public string? Name { get; set; }
}
