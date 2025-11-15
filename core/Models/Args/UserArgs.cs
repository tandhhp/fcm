using Waffle.Entities.Payments;

namespace Waffle.Models.Args;

public class UserArgs
{
}

public class LoyaltyAddArgs
{
    public Guid UserId { get; set; }
    public int Point { get; set; }
    public string? Memo { get; set; }
    public DateTime ExpiredDate { get; set; }
    public TransactionType? Type { get; set; } = TransactionType.Default;
}

public class ForgotPasswordArgs
{
    public string? Email { get; set; }
}

public class SetSaller
{
    public Guid CardHolderId { get; set; }
    public Guid SellerId { get; set; }
}

public class ChangeContractCodeArgs
{
    public Guid? UserId { get; set; }
    public string? ContractCode { get; set; }
}