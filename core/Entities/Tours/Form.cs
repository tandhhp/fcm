namespace Waffle.Entities.Tours;

public class Form : BaseEntity
{
    public DateTime? ScheduledDate { get; set; }
    public int Room { get; set; }
    public Guid? CatalogId { get; set; }
    public int Adult { get; set; }
    public int Children { get; set; }
    public FormStatus Status { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }
    public int Point { get; set; }
    public string? Note { get; set; }
    public Guid? AccountantId { get; set; }
    public DateTime? AccountantApprovedDate { get; set; }
}

public enum FormStatus
{
    New,
    InProgress,
    Completed,
    Canceled,
    AccountantApproved
}
