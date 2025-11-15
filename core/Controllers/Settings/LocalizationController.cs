using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Settings;

public class LocalizationController : BaseController
{
    private readonly ILocalizationService _localizationService;
    public LocalizationController(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync([FromQuery] LocalizationFilterOptions filterOptions)
    {
        if (string.IsNullOrWhiteSpace(filterOptions.Locale)) return BadRequest("Language code empty!");
        return Ok(await _localizationService.GetListAsync(filterOptions));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id) => Ok(await _localizationService.GetAsync(id));

    [HttpPost("save")]
    public async Task<IActionResult> SaveAsync([FromBody] Localization args)
    {
        if (string.IsNullOrWhiteSpace(args.Value)) return BadRequest("Value is required!");
        args.Value = args.Value.Trim();
        return Ok(await _localizationService.SaveAsync(args));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] Localization args)
    {
        if (string.IsNullOrWhiteSpace(args.Value) || string.IsNullOrWhiteSpace(args.Key)) return BadRequest("Key or Value empty!");
        args.Value = args.Value.Trim();
        args.Key = args.Key.Trim();
        return Ok(await _localizationService.AddAsync(args));
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _localizationService.DeleteAsync(id));
}
