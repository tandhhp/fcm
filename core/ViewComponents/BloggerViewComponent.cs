using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents;

public class BloggerViewComponent : ViewComponent
{
    private readonly ISettingService _appService;
    private readonly IGoogleService _googleService;
    public BloggerViewComponent(ISettingService appService, IGoogleService googleService)
    {
        _appService = appService;
        _googleService = googleService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Blogger args)
    {
        var blogger = await _appService.GetAsync<ExternalAPI.GoogleAggregate.Google>(nameof(Google));
        if (blogger is null)
        {
            return View(Empty.DefaultView, new ErrorViewModel
            {
                RequestId = $"{nameof(Blogger)}: {args.BlogId}/{args.PostId}"
            });
        }
        var post = await _googleService.BloggerGetAsync(args.BlogId, args.PostId, blogger.BloggerApiKey);
        return View("~/Pages/Shared/Components/Html/Default.cshtml", new Html
        {
            Value = post?.Content
        });
    }
}
