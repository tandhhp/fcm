using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Core.Services.Sources.Filters;

public class TypeOfDataSelectOptions : SelectOptions
{
    public SourceType? SourceType { get; set; }
}

public class SourceSelectOptions : SelectOptions
{
    public int? TeamId { get; set; }
    public TypeOfDataSelectType? TypeOfData { get; set; }
}

public class SourceTeamSelectOptions : SelectOptions
{
    public TypeOfDataSelectType TypeOfData { get; set; }
}

public enum TypeOfDataSelectType
{
    New = 1,
    Old = 2,
    StartCase = 3
}