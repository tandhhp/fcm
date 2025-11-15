using Waffle.Entities.Payments;

namespace Waffle.Models.Args.Users;

public class ApproveLoanArgs
{
    public Guid TransactionId { get; set; }
    public TransactionStatus Status { get; set; }
    public string? Reason { get; set; }
}
