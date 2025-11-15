namespace Waffle.Entities.Payments;

public class Transaction : BaseEntity
{
    public int Point { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Memo { get; set; }
    public Guid UserId { get; set; }
    public Guid? FormId { get; set; }
    public TransactionStatus Status { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? Reason { get; set; }
    public TransactionType? Type { get; set; }
    public string? Feedback { get; set; }
}

public enum TransactionStatus
{
    None,
    Pending,
    Approved,
    Reject
}

public enum TransactionType
{
    Default,
    Bonus,
    Loan
}