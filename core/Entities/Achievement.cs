namespace Waffle.Entities;

public class Achievement : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Icon { get; set; } = default!;
    public string NormalizedName { get; set; } = default!;
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? CxId { get; set; }
    public bool IsApproved { get; set; }
    public Guid? CxmId { get; set; }
    public DateTime? ApprovedDate { get; set; }
}
