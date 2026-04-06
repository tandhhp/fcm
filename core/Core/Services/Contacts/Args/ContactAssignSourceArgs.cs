namespace Waffle.Core.Services.Contacts.Args;

public class ContactAssignSourceArgs
{
    public Guid TelesalesId { get; set; }
    public int NumberOfContact { get; set; }
    public int SourceId { get; set; }
}

public class ReportDataSourceFilterOptions
{
    public int? TeamId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? SourceId { get; set; }
}

public class TmrDataReportFilterOptions
{
    public int? TeamId { get; set; }
    public TmrDataReportViewType ViewType { get; set; }
}

public enum TmrDataReportViewType
{
    Assigned,
    CallStatus
}

public class TmrUserData
{
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public int TotalAssign { get; set; }
    public int TotalAvailableContact { get; set; }
    public int TotalTeleNotUpdate { get; set; }
    public int TotalTemporaryLockedWrongNumberKNM { get; set; }
    public int TotalNotEnoughQualify { get; set; }
    public int TotalMeetRequire { get; set; }
    public int TotalRefuseToTalk { get; set; }
    public int TotalLocation { get; set; }
}

public class TmrTeamData
{
    public string? LeaderName { get; set; }
    public List<TmrUserData> Users { get; set; } = new List<TmrUserData>();

    // Tự động tính tổng dựa trên danh sách user bên dưới

    // Nếu view type là Assigned thì hiển thị nhưng thông tin này
    public int TotalAssign => Users.Sum(u => u.TotalAssign);
    public int TotalAvailableContact => Users.Sum(u => u.TotalAvailableContact);

    // Nếu view type là CallStatus thì hiển thị nhưng thông tin này
    public int TotalTeleNotUpdate => Users.Sum(u => u.TotalTeleNotUpdate);
    public int TotalTemporaryLockedWrongNumberKNM => Users.Sum(u => u.TotalTemporaryLockedWrongNumberKNM);
    public int TotalNotEnoughQualify => Users.Sum(u => u.TotalNotEnoughQualify);
    public int TotalMeetRequire => Users.Sum(u => u.TotalMeetRequire);
    public int TotalRefuseToTalk => Users.Sum(u => u.TotalRefuseToTalk);
    public int TotalLocation => Users.Sum(u => u.TotalLocation);
}