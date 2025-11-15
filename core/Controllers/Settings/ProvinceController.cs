using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Provinces.Models;
using Waffle.Foundations;
using Waffle.Models.Settings.Provinces;

namespace Waffle.Controllers.Settings;

public class ProvinceController(IProvinceService _provinceService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] ProvinceFilterOptions filterOptions) => Ok(await _provinceService.ListAsync(filterOptions));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] string? keyWords) => Ok(await _provinceService.OptionsAsync(keyWords));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProvinceCreateArgs args) => Ok(await _provinceService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ProvinceUpdateArgs args) => Ok(await _provinceService.UpdateAsync(args));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id) => Ok(await _provinceService.DeleteAsync(id));
}
