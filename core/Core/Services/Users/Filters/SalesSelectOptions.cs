using Waffle.Models;

namespace Waffle.Core.Services.Users.Filters;

public class SalesSelectOptions : SelectOptions
{
    public Guid? SalesManagerId { get; set; }
}
