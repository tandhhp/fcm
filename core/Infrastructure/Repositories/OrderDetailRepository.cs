using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Data;
using Waffle.Entities.Ecommerces;
using Waffle.Models.ViewModels.Orders;

namespace Waffle.Infrastructure.Repositories;

public class OrderDetailRepository : EfRepository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async IAsyncEnumerable<OrderProductItem> ListByOrderAsync(Guid orderId)
    {
        var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
        foreach (var item in orderDetails)
        {
            var product = await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product is null) continue;
            yield return new OrderProductItem
            {
                Id = item.Id,
                ProductId = item.ProductId,
                OrderId = item.OrderId,
                Price = item.Price,
                Quantity = item.Quantity,
                ProductName = product.Name
            };
        }
    }
}
