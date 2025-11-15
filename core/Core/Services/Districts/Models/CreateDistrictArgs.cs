namespace Waffle.Core.Services.Districts.Models;

public class CreateDistrictArgs
{
    public int ProvinceId { get; set; }
    public string Name { get; set; } = default!;
}
