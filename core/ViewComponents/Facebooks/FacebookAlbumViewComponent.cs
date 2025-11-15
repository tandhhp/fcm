using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Models.Components;

namespace Waffle.ViewComponents.Facebooks;

public class FacebookAlbumViewComponent : BaseViewComponent<FacebookAlbum>
{
    private readonly IFacebookService _facebookService;
    private readonly ISettingService _appSettingService;

    public FacebookAlbumViewComponent(IWorkService workService, IFacebookService facebookService, ISettingService appSettingService) : base(workService)
    {
        _facebookService = facebookService;
        _appSettingService = appSettingService;
    }

    protected override async Task<FacebookAlbum> ExtendAsync(FacebookAlbum work)
    {
        var setting = await _appSettingService.GetAsync<Facebook>(nameof(Facebook));
        if (setting == null) return work;
        var access_token = setting.PageAccessToken;
        var before = Request.Query["before"].ToString();
        var after = Request.Query["after"].ToString();
        var result = await _facebookService.GetPhotosAsync(work.AlbumId, 12, before, after, access_token);

        work.Photos = result.Data;
        work.Before = result.Paging?.Cursors?.Before;
        work.After = result.Paging?.Cursors?.After;
        return work;
    }
}
