using Waffle.Models;

namespace Waffle.Core.Services.Roles.Filters;

public class TelesalesSelectOptions : SelectOptions
{
    public Guid? DotId { get; set; }
    public Guid? TelesalesManagerId { get; set; }
}

public class KeyInSelectOptions : SelectOptions
{
    public Guid? TeamKeyInId { get; set; }
}