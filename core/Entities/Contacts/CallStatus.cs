using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class CallStatus : BaseEntity<int>
{
    [StringLength(512)]
    public string Name { get; set; } = default!;
    [StringLength(64)]
    public string? Code { get; set; } = default!;
    public CallStatusType? Type { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<CallHistory>? CallHistories { get; set; }
}

public enum CallStatusType
{
    TELE_NOT_UPDATE = 1,
    TEMPORARY_LOCKED_WRONG_NUMBER_KNM = 2,
    NOT_ENOUGH_QUALIFY = 3,
    MEET_REQUIRE = 4,
    REFUSE_TO_TALK = 5,
    LOCATION = 6
}