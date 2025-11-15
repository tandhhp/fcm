using System.Text.Json.Serialization;
using Waffle.Entities;
using Waffle.Entities.Ecommerces;

namespace Waffle.Models.Params.Products;

public class CartItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Catalog? Catalog { get; set; }
    public Product? Product { get; set; }
}
