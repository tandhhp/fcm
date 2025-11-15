using Waffle.Entities.Contracts;
using Waffle.Models;

namespace Waffle.Core.Services.Events.Models;

public class VoucherFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public bool? IsUsed { get; set; }
    public bool? IsExpired { get; set; }
    public VoucherStatus? Status { get; set; }
}
