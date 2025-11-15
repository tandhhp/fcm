namespace Waffle.Entities.Ecommerces;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public string Number { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? Note { get; set; }
}

public enum OrderStatus
{
    Open,
    Confirmed,
    Paid,
    Refunded,
    Cancelled
}
