using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.ECommerces;

public class ProductPickerViewComponent : BaseViewComponent<ProductPicker>
{
    private readonly ICatalogService _catalogService;

    public ProductPickerViewComponent(IWorkService workService, ICatalogService catalogService) : base(workService)
    {
        _catalogService = catalogService;
    }

    protected override async Task<ProductPicker> ExtendAsync(ProductPicker work)
    {
        if (!work.TagIds.Any()) return work;
        var products = await _catalogService.ListByTagAsync(work.TagIds.First(), new CatalogFilterOptions
        {
            Type = CatalogType.Product,
            Active = true,
            PageSize = 4
        });
        work.Products = products.Data;
        return work;
    }
}
