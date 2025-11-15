using Waffle.Entities.Ecommerces;

namespace Waffle.Models.ViewModels.Orders;

public class OrderDetailViewModel : Order
{
    public string? CustomerName { get; set; }
    public IAsyncEnumerable<OrderProductItem> OrderDetails { get; set; } = default!;
}

public class OrderProductItem : OrderDetail
{
    public string? ProductName { get; set; }
}