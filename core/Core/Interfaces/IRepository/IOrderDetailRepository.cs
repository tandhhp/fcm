using Waffle.Entities.Ecommerces;
using Waffle.Models.ViewModels.Orders;

namespace Waffle.Core.Interfaces.IRepository;

public interface IOrderDetailRepository : IAsyncRepository<OrderDetail>
{
    IAsyncEnumerable<OrderProductItem> ListByOrderAsync(Guid id);
}
