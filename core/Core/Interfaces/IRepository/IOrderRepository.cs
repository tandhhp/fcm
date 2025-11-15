using Waffle.Entities.Ecommerces;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IRepository;

public interface IOrderRepository : IAsyncRepository<Order>
{
    Task<int> CountAsync(OrderStatus status);
    Task<ListResult<Order>> ListAsync(IFilterOptions filterOptions);
    Task<IEnumerable<OrderDetail>> ListOrderDetails(Guid id);
    void RemoveRange(IEnumerable<OrderDetail> orderDetails);
}
