using Waffle.Models;

namespace Waffle.Core.Services.Users.Filters;

public class TelesaleFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int? TeamId { get; set; }
    public string? LineCode { get; set; }
}
