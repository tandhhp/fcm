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
    /// <summary>
    /// Mới tinh, chưa sử dụng cho tele nào hết
    /// </summary>
    New = 1,
    /// <summary>
    /// Chia cho tele rồi và trong 1 tháng trở về trước
    /// </summary>
    Old = 2,
    /// <summary>
    /// Nguồn dữ liệu đã sử dụng đã phân cho tele
    /// </summary>
    StartCase = 3
}

public class SourceContactFilterOptions : FilterOptions
{
    public TypeOfDataSelectType TypeOfData { get; set; }
    public int? TeamId { get; set; }
    public string? SourceIds { get; set; }
    public int? CallStatusId { get; set; }
    public CallStatusType? CallStatusType { get; set; }
    public string? ExtraStatus { get; set; }
    public string? PhoneNumber { get; set; }
}

public class SourceMultipleAssignArgs
{
    // Lấy nguồn ở đâu
    public List<int>? SourceIds { get; set; }
    public TypeOfDataSelectType? TypeOfData { get; set; }
    public int? CallStatusId { get; set; }
    public CallStatusType? CallStatusType { get; set; }
    public string? ExtraStatus { get; set; }
    public int ContactCount { get; set; }
    // Gán cho ai
    public List<Guid>? TeleIds { get; set; }
}