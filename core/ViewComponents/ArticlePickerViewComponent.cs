using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class ArticlePickerViewComponent : BaseViewComponent<ArticlePicker>
{
    private readonly ICatalogService _catalogService;

    public ArticlePickerViewComponent(IWorkService workService, ICatalogService catalogService) : base(workService)
    {
        _catalogService = catalogService;
    }

    protected override async Task<ArticlePicker> ExtendAsync(ArticlePicker work)
    {
        var articles = await _catalogService.ListByTagAsync(work.TagId, new CatalogFilterOptions
        {
            Type = CatalogType.Article,
            Active = true,
            PageSize = 5,
            Locale = PageData.Locale
        });
        work.Articles = articles.Data?.ToList() ?? new();
        return work;
    }
}
