using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Sources.Args;
using Waffle.Core.Services.Sources.Filters;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers;

public class SourceController(ISourceService _sourceService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] SourceFilterOptions filterOptions) => Ok(await _sourceService.ListAsync(filterOptions));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> DetailAsync([FromRoute] int id) => Ok(await _sourceService.DetailAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] SourceCreateArgs args) => Ok(await _sourceService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] SourceUpdateArgs args) => Ok(await _sourceService.UpdateAsync(args));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id) => Ok(await _sourceService.DeleteAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _sourceService.OptionsAsync(selectOptions));

    [HttpGet("report")]
    public async Task<IActionResult> ReportAsync([FromQuery] FilterOptions filterOptions) => Ok(await _sourceService.ReportAsync(filterOptions));

    [HttpGet("availables")]
    public async Task<IActionResult> AvailablesAsync() => Ok(await _sourceService.AvailablesAsync());

    [HttpPost("assign")]
    public async Task<IActionResult> AssignAsync([FromBody] SourceAssignArgs args) => Ok(await _sourceService.AssignAsync(args));
}