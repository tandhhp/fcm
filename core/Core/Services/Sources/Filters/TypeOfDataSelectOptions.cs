using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Sources.Filters;

public class TypeOfDataSelectOptions : SelectOptions
{
    public SourceType? SourceType { get; set; }
}
