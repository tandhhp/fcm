using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Extensions;
using Waffle.Models;
using Waffle.Models.Components;
using Waffle.Models.Components.Lister;

namespace Waffle.Pages.Videos;

public class IndexModel : EntryPageModel
{
    public IndexModel(ICatalogService catalogService) : base(catalogService) { }

    public List<PlaylistItem> PlaylistItems = new();
    public Pagination Pagination { get; set; } = new Pagination();

    public async Task<IActionResult> OnGetAsync()
    {
        var current = string.IsNullOrEmpty(Request.Query["current"]) ? 1 : int.Parse(Request.Query["current"].ToString());
        var videos = await _catalogService.ListAsync(new CatalogFilterOptions
        {
            Active = true,
            PageSize = 12,
            Type = CatalogType.Video,
            Current = current
        });
        Pagination = videos.Pagination;
        PlaylistItems = videos.Data?.Select(x => new PlaylistItem
        {
            Name = x.Name,
            Date = x.ModifiedDate.ToString("D"),
            Thumbnail = x.Thumbnail,
            ViewCount = x.ViewCount.ToNumber(),
            Url = x.GetUrl()
        }).ToList() ?? new();

        return Page();
    }
}
