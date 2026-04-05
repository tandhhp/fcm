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