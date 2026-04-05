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

public class SourceContactFilterOptions : FilterOptions
{
    public TypeOfDataSelectType TypeOfData { get; set; }
    public int? TeamId { get; set; }
    public string? SourceIds { get; set; }
    public int? CallStatusId { get; set; }
    public string? ExtraStatus { get; set; }
}

public class SourceMultipleAssignArgs
{
    // Lấy nguồn ở đâu
    public List<int>? SourceIds { get; set; }
    public TypeOfDataSelectType? TypeOfData { get; set; }
    public int? CallStatusId { get; set; }
    public string? ExtraStatus { get; set; }
    public int ContactCount { get; set; }
    // Gán cho ai
    public List<Guid>? TeleIds { get; set; }
}