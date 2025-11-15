using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Settings.Models;
using Waffle.Data;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Components;
using Waffle.Models.Settings;
using Waffle.UnitTest;
using WFSendGrid = Waffle.ExternalAPI.SendGrids.SendGrid;

namespace Waffle.Controllers;

public class SettingController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly ISettingService _settingService;
    private readonly IConfiguration _configuration;
    private readonly IFacebookService _facebookService;
    private readonly IWorkService _workService;
    private readonly IEmailSender _emailSender;

    public SettingController(IEmailSender emailSender, ApplicationDbContext context, ISettingService appSettingService, IConfiguration configuration, IFacebookService facebookService, IWorkService workService)
    {
        _context = context;
        _settingService = appSettingService;
        _configuration = configuration;
        _facebookService = facebookService;
        _workService = workService;
        _emailSender = emailSender;
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync([FromRoute] string name) => Ok(TResult<object>.Ok(await _settingService.GetAsync<object>(name)));

    [HttpGet("header/{id}")]
    public async Task<IActionResult> GetHeaderAsync([FromRoute] Guid id) => Ok(await _settingService.GetAsync<Header>(id) ?? new Header());

    [HttpGet("footer/{id}")]
    public async Task<IActionResult> GetFooterAsync([FromRoute] Guid id) => Ok(await _settingService.GetAsync<Footer>(id) ?? new Footer());

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync() => Ok(await _settingService.ListAsync());

    [HttpPost("save/{id}")]
    public async Task<IActionResult> SaveAsync([FromRoute] Guid id, [FromBody] object args) => Ok(await _settingService.SaveAsync(id, args));

    [HttpPost("unix/save/{normalizedName}")]
    public async Task<IActionResult> SaveAsync([FromRoute] string normalizedName, [FromBody] object args) => Ok(await _settingService.SaveAsync(normalizedName, args));

    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            language = _configuration.GetValue<string>("language")
        });
    }

    [HttpGet("sendgrid")]
    public async Task<IActionResult> GetSendGridAsync()
    {
        var app = await _settingService.EnsureSettingAsync(nameof(SendGrid));
        return Ok(await _settingService.GetAsync<WFSendGrid>(app.Id));
    }

    [HttpPost("sendgrid/save")]
    public async Task<IActionResult> SaveSendGridAsync([FromBody] WFSendGrid args)
    {
        var app = await _settingService.EnsureSettingAsync(nameof(SendGrid));
        app.Value = JsonSerializer.Serialize(args);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("layout/{id}")]
    public async Task<IActionResult> GetLayoutAsync([FromRoute] Guid id)
    {
        var setting = await _context.AppSettings.FindAsync(id);
        if (setting is null)
        {
            return Ok(IdentityResult.Failed());
        }
        var query = from a in _context.WorkItems
                    join b in _context.WorkContents on a.CatalogId equals setting.Id
                    where a.CatalogId == setting.Id
                    orderby a.SortOrder ascending
                    select b;
        return Ok(new
        {
            data = await query.ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpGet("facebook/{id}")]
    public async Task<IActionResult> FacebookGetAsync([FromRoute] Guid id) => Ok(await _settingService.GetAsync<Facebook>(id));

    [HttpPost("facebook/save")]
    public async Task<IActionResult> SaveFacebookAsync([FromBody] Facebook model)
    {
        var app = await _settingService.EnsureSettingAsync(nameof(Facebook));
        var facebook = await _settingService.GetAsync<Facebook>(app.Id);
        if (facebook != null)
        {
            var userToken = await _facebookService.GetLongLivedUserAccessTokenAsync(facebook.AppId, facebook.AppSecret, model.ShortLiveToken);
            if (userToken != null)
            {
                facebook.LongLivedUserAccessToken = userToken;
                var pageToken = await _facebookService.GetLongLivedPageAccessTokenAsync(facebook.PageId, facebook.LongLivedUserAccessToken.AccessToken);
                if (pageToken != null)
                {
                    facebook.PageAccessToken = pageToken.AccessToken;
                }
            }
            facebook.AppId = model.AppId;
            facebook.AppSecret = model.AppSecret;
            facebook.PageId = model.PageId;
        }
        else
        {
            facebook = model;
        }
        app.Value = JsonSerializer.Serialize(facebook);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("long-lived-user-access-token")]
    public async Task<IActionResult> GetLongLivedUserAccessTokenAsync([FromQuery] string shortLiveToken)
    {
        var app = await _settingService.GetAsync<Facebook>(nameof(Facebook));
        if (app is null)
        {
            return NotFound();
        }
        return Ok(await _facebookService.GetLongLivedUserAccessTokenAsync(app.AppId, app.AppSecret, shortLiveToken));
    }

    [HttpPost("telegram/save/{id}")]
    public async Task<IActionResult> SaveTelegramAsync([FromRoute] Guid id, [FromBody] Telegram args) => Ok(await _settingService.SaveTelegramAsync(id, args));

    [HttpGet("telegram/{id}")]
    public async Task<IActionResult> GetTelegramAsync([FromRoute] Guid id) => Ok(await _settingService.GetAsync<Telegram>(id));

    [HttpPost("telegram/test")]
    public async Task<IActionResult> TestTelegramAsync([FromBody] TelegramMessage message)
    {
        var telegram = await _settingService.GetAsync<Telegram>(nameof(Telegram));
        if (telegram is null)
        {
            return Ok(IdentityResult.Failed(new IdentityError
            {
                Description = "Config not found"
            }));
        }
        return Ok(IdentityResult.Success);
    }

    [HttpGet("social/{id}")]
    public async Task<IActionResult> GetSocialLinkAsync([FromRoute] Guid id) => Ok(await _settingService.GetAsync<Social>(id));

    [HttpPost("social/save")]
    public async Task<IActionResult> SaveSocialLinkAsync([FromBody] Social args)
    {
        var setting = await _settingService.EnsureSettingAsync(nameof(Social));
        setting.Value = JsonSerializer.Serialize(args);
        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpGet("sidebar")]
    public async Task<IActionResult> GetSidebarAsync()
    {
        var sidebar = await _settingService.EnsureSettingAsync(nameof(Sidebar));
        var components = await _workService.ListBySettingIdAsync(sidebar.Id);
        return Ok(components);
    }

    [HttpPost("test-send-mail")]
    public async Task<IActionResult> TestSendMailAsync([FromBody] EmailSenderMessageUnitTest args) => Ok(await _emailSender.SendEmailAsync(args.Email, args.Subject, args.Message));

    [HttpGet("graph-api-explorer")]
    public async Task<IActionResult> GraphAPIExplorerAsync([FromQuery] string query)
    {
        return Ok(await _facebookService.GraphAPIExplorerAsync(query));
    }

    [HttpGet("themes/options")]
    public IActionResult GetThemeOptionsAsync() => Ok(Themes.All);

    [HttpGet("blogger/blog-list")]
    public async Task<IActionResult> GetBloggerListAsync()
    {
        var google = await _settingService.GetAsync<ExternalAPI.GoogleAggregate.Google>(nameof(Google));
        if (google is null) return BadRequest();
        return Ok(google.Bloggers);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveAsync([FromBody] SaveSettingArgs args) => Ok(await _settingService.SaveAsync(args));
}
