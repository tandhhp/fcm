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
    [Display(Name = "Tele not update")]
    TELE_NOT_UPDATE = 1,
    [Display(Name = "Tele locked / Wrong number / NM")]
    TEMPORARY_LOCKED_WRONG_NUMBER_KNM = 2,
    [Display(Name = "Not enough qualify")]
    NOT_ENOUGH_QUALIFY = 3,
    [Display(Name = "Meet require")]
    MEET_REQUIRE = 4,
    [Display(Name = "Refuse to talk")]
    REFUSE_TO_TALK = 5,
    [Display(Name = "Location")]
    LOCATION = 6
}

public static class CallStatusCode
{
    /// <summary>
    /// Tele not update
    /// </summary>
    public const string TELE_NOT_UPDATE = nameof(TELE_NOT_UPDATE);
    /// <summary>
    /// Thuê bao
    /// </summary>
    public const string NOT_AVAILABLE = nameof(TELE_NOT_UPDATE);
    /// <summary>
    /// Không nghe máy
    /// </summary>
    public const string NO_ANSWER = nameof(NO_ANSWER);
    /// <summary>
    /// Sai số
    /// </summary>
    public const string WRONG_NUMBER = nameof(WRONG_NUMBER);
    /// <summary>
    /// Thói quen kém
    /// </summary>
    public const string POOR_HABIT = nameof(POOR_HABIT);
    /// <summary>
    /// Kinh tế kém
    /// </summary>
    public const string POOR_FINANCIAL = nameof(POOR_FINANCIAL);
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
    public const string CALL_UNDER_15S = nameof(CALL_UNDER_15S);
    /// <summary>
    /// Không quan tâm
    /// </summary>
    public const string NOT_INTERESTED = nameof(NOT_INTERESTED);
    /// <summary>
    /// Location
    /// </summary>
    public const string LOCATION = nameof(LOCATION);
}