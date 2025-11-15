namespace Waffle.Entities.Contracts;

public class ContractGift
{
    public Guid ContractId { get; set; }
    public Guid GiftId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public Contract? Contract { get; set; }
    public Gift? Gift { get; set; }
}
