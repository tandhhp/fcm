using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Waffle.Core.Options;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models.GoogleAggregate;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class TrendViewComponent : ViewComponent
{
    private readonly IGoogleService _googleService;
    private readonly SettingOptions Options;
    public TrendViewComponent(IGoogleService googleService, IOptions<SettingOptions> options)
    {
        _googleService = googleService;
        Options = options.Value;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid id) {
        if (Options.Theme != "Default")
        {
            return View("~/Pages/Components/Empty/NoContent.cshtml");
        }
        var trend = await _googleService.GetDailyTrendingAsync();
        if (trend is null || trend.Channel is null || trend.Channel.Item is null)
        {
            return View(Empty.DefaultView, new ErrorViewModel
            {
                RequestId = id.ToString()
            });
        }
        return View("~/Pages/Shared/Components/ListGroup/Default.cshtml", new ListGroup
        {
            Name = "Daily Trending",
            Items = GetItems(trend.Channel.Item)
        });
    }

    private static IEnumerable<ListGroupItem> GetItems(List<ChannelItem> items)
    {
        foreach (var item in items)
        {
            yield return new ListGroupItem
            {
                Link = new Link
                {
                    Href = $"/search?searchTerm={item.Title}",
                    Name = item.Title ?? string.Empty
                }
            };
        }
    }
}
