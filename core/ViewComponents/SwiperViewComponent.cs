using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Extensions;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class SwiperViewComponent : BaseViewComponent<Swiper>
{
    private readonly ICatalogService _catalogService;
    public SwiperViewComponent(IWorkService workService, ICatalogService catalogService) : base(workService)
    {
        _catalogService = catalogService;
    }

    protected override async Task<Swiper> ExtendAsync(Swiper work)
    {
        var albums = await _catalogService.ListAsync(new CatalogFilterOptions
        {
            Active = true,
            Type = CatalogType.Album
        });
        if (albums.Data != null)
        {
            work.Items = albums.Data.Select(x => new SwiperItem
            {
                Title = x.Name,
                Image = x.Thumbnail,
                Description = x.Description,
                Url = x.GetUrl()
            }).ToList();
        }
        return work;
    }
}
