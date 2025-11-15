using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Extensions;
using Waffle.Models;
using Waffle.Models.Components.Lister;

namespace Waffle.ViewComponents;

public class VideoPlayListViewComponent : BaseViewComponent<VideoPlayList>
{
    private readonly ICatalogService _catalogService;
    public VideoPlayListViewComponent(IWorkService workService, ICatalogService catalogService) : base(workService)
    {
        _catalogService = catalogService;
    }

    protected override async Task<VideoPlayList> ExtendAsync(VideoPlayList work)
    {
        var current = string.IsNullOrEmpty(Request.Query["current"]) ? 1 : int.Parse(Request.Query["current"].ToString());
        var videos = await _catalogService.ListAsync(new CatalogFilterOptions
        {
            Active = true,
            Type = CatalogType.Video,
            PageSize = work.PageSize,
            Current = current
        });
        work.Pagination = videos.Pagination;
        work.PlaylistItems = videos?.Data?.Select(x => new PlaylistItem
        {
            Name = x.Name,
            Thumbnail = x.Thumbnail,
            Url = $"/video/{x.NormalizedName}",
            Date = x.ModifiedDate.ToString("f"),
            ViewCount = x.ViewCount.ToNumber(),
        }).ToList() ?? new();
        return work;
    }
}
