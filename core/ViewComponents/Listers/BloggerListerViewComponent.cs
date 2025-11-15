using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.Listers;

public class BloggerListerViewComponent : BaseViewComponent<BloggerLister>
{
    private readonly IGoogleService _googleService;
    private readonly ISettingService _settingService;
    public BloggerListerViewComponent(IWorkService workService, IGoogleService googleService, ISettingService settingService) : base(workService)
    {
        _googleService = googleService;
        _settingService = settingService;
    }

    protected override async Task<BloggerLister> ExtendAsync(BloggerLister work)
    {
        var google = await _settingService.GetAsync<ExternalAPI.GoogleAggregate.Google>(nameof(Google));
        if (google is null) return work;
        work.Posts = await _googleService.BloggerPostsAsync(work.BlogId, google.BloggerApiKey, work.PageSize, Request.Query["pageToken"].ToString(), string.Empty);
        work.Category = PageData.NormalizedName;
        return work;
    }
}
