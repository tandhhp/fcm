using Waffle.Entities.Ecommerces;

namespace Waffle.Models.Params.Products;

public class AddOrderRequest : Order
{
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = new();
}
