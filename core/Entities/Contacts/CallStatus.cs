using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class CallStatus : BaseEntity<int>
{
    [StringLength(512)]
    public string Name { get; set; } = default!;
    [StringLength(64)]
    public string? Code { get; set; } = default!;
    public CallStatusType? Type { get; set; }
    public int SortOrder { get; set; }

    public virtual ICollection<CallHistory>? CallHistories { get; set; }
}

public enum CallStatusType
{
    [Display(Name = "Không nghe máy")]
    NO_PICK_UP = 1,
    [Display(Name = "Thuê bao")]
    TEMPORARY_LOCKED = 2,
    [Display(Name = "Sai số")]
    WRONG_NUMBER = 3,
    [Display(Name = "Ngoại tỉnh")]
    LOCATION = 4,
    [Display(Name = "Gọi lại sau")]
    CALL_LATER = 5,
    [Display(Name = "Khách đạt yêu cầu")]
    MEET_REQUIRE = 6,
    [Display(Name = "Khách không đạt yêu cầu")]
    NOT_ENOUGH_QUALIFY = 7
}

public static class CallStatusCode
{
    /// <summary>
    /// Tele not update
    /// </summary>
    public const string NO_PICK_UP = nameof(NO_PICK_UP);
    /// <summary>
    /// Thuê bao
    /// </summary>
    public const string NOT_AVAILABLE = nameof(NOT_AVAILABLE);
    /// <summary>
    /// Không nghe máy
    /// </summary>
    public const string NO_ANSWER = nameof(NO_ANSWER);
    /// <summary>
    /// Sai số
    /// </summary>
    public const string WRONG_NUMBER = nameof(WRONG_NUMBER);
    /// <summary>
    /// Confirm 1
    /// </summary>
    public const string CONFIRM1 = nameof(CONFIRM1);
    /// <summary>
    /// Consider
    /// </summary>
    public const string CONSIDER = nameof(CONSIDER);
    /// <summary>
    /// Another time
    /// </summary>
    public const string ANOTHER_TIME = nameof(ANOTHER_TIME);
    /// <summary>
    /// Gọi lại sau
    /// </summary>
    public const string CALL_LATER = nameof(CALL_LATER);
    /// <summary>
    /// Cuộc gọi dưới 15s
    /// </summary>
    public const string UNDER15S = nameof(UNDER15S);
    /// <summary>
    /// Cuộc gọi trên 15s
    /// </summary>
    public const string OVER15S = nameof(OVER15S);
    /// <summary>
    /// Ngoại tỉnh
    /// </summary>
    public const string LOCATION = nameof(LOCATION);
}