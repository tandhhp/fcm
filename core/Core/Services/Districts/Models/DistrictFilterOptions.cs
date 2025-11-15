using Waffle.Models;

namespace Waffle.Core.Services.Districts.Models;

public class DistrictFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public int? ProvinceId { get; set; }
}
