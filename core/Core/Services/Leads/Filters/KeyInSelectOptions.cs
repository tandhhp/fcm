using Waffle.Models;

namespace Waffle.Core.Services.Leads.Filters;

public class KeyInSelectOptions : SelectOptions
{
    public Guid? ManagerId { get; set; }
}
