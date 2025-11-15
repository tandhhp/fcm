using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Districts.Models;
using Waffle.Foundations;

namespace Waffle.Controllers.Settings;

public class DistrictController(IDistrictService _districtService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] DistrictFilterOptions filterOptions) => Ok(await _districtService.ListAsync(filterOptions));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] DistrictSelectOptions selectOptions) => Ok(await _districtService.OptionsAsync(selectOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDistrictArgs args) => Ok(await _districtService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateDistrictArgs args) => Ok(await _districtService.UpdateAsync(args));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id) => Ok(await _districtService.DeleteAsync(id));
}
