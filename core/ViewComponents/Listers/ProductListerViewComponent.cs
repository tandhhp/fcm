using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.Listers;

public class ProductListerViewComponent : BaseViewComponent<ProductLister>
{
    private readonly IProductService _productService;
    public ProductListerViewComponent(IWorkService workService, IProductService productService) : base(workService)
    {
        _productService = productService;
    }

    protected override async Task<ProductLister> ExtendAsync(ProductLister work)
    {
        var current = string.IsNullOrEmpty(Request.Query["current"]) ? 1 : int.Parse(Request.Query["current"].ToString());

        work.Products = await _productService.ListAsync(new ProductFilterOptions
        {
            Active = true,
            Current = current,
            PageSize = work.PageSize,
            Name = Request.Query["searchTerm"],
            ParentId = PageData.Type == CatalogType.Entry ? null : PageData.Id
        });
        if (string.IsNullOrEmpty(work.Title))
        {
            work.Title = PageData.Name;
        }
        return work;
    }
}
