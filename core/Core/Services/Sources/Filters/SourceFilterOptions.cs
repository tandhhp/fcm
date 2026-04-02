using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Sources.Filters;

public class SourceFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public int? TypeOfDataId { get; set; }
    public int? TeamId { get; set; }
    public SourceType? SourceType { get; set; }
}
