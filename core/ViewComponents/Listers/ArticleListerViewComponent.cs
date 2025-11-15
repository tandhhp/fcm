using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.Listers;

public class ArticleListerViewComponent : BaseViewComponent<ArticleLister>
{
    private readonly ICatalogService _catalogService;
    
    public ArticleListerViewComponent(IWorkService workService, ICatalogService catalogService) : base(workService)
    {
        _catalogService = catalogService;
    }

    protected override async Task<ArticleLister> ExtendAsync(ArticleLister work)
    {
        var current = string.IsNullOrEmpty(Request.Query["current"]) ? 1 : int.Parse(Request.Query["current"].ToString());
        var articles = await _catalogService.ListAsync(new CatalogFilterOptions
        {
            Active = true,
            Current = current,
            PageSize = work.PageSize,
            ParentId = PageData.Type == CatalogType.Entry ? null : PageData.Id,
            Type = CatalogType.Article,
            Name = Request.Query["searchTerm"],
            Locale = PageData.Locale
        });
        work.Articles = articles;
        if (string.IsNullOrEmpty(work.Name))
        {
            work.Name = PageData.Name;
        }
        return work;
    }
}
