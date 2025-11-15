using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Foundations;
using Waffle.Models.Settings;

namespace Waffle.Controllers.Settings;

public class StyleController : BaseController
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;
    private readonly ISettingService _settingService;

    public StyleController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ISettingService settingService)
    {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
        _settingService = settingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var path = await GetPathAsync();
        CreateFile(path);
        return Ok(await System.IO.File.ReadAllTextAsync(await GetPathAsync()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var path = Path.Combine(_webHostEnvironment.WebRootPath, "css", $"{id}.css");
        if (!System.IO.File.Exists(path))
        {
            var file = System.IO.File.Create(path);
            file.Close();
        }
        return Ok(await System.IO.File.ReadAllTextAsync(path));
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveAsync([FromBody] WorkContent workItem)
    {
        CreateFile(await GetPathAsync());
        await System.IO.File.WriteAllTextAsync(await GetPathAsync(), workItem.Arguments);
        return Ok(IdentityResult.Success);
    }

    private static void CreateFile(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            var file = System.IO.File.Create(path);
            file.Close();
        }
    }

    private async Task<string> GetPathAsync()
    {
        var setting = await _settingService.GetAsync<Theme>(nameof(Theme));
        setting ??= new Theme
            {
                Name = "default"
            };
        return Path.Combine(_webHostEnvironment.WebRootPath, "css", $"{setting.Name}.css");
    }
}
