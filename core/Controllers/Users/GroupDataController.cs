using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Core.Services.Teams.Models;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Users;

[Route("api/group-data")]
public class GroupDataController(IGroupDataService _groupDataService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] GroupDataFilterOptions filterOptions) => Ok(await _groupDataService.ListAsync(filterOptions));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> DetailAsync([FromRoute] int id) => Ok(await _groupDataService.DetailAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGroupDataArgs args) => Ok(await _groupDataService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateGroupDataArgs args) => Ok(await _groupDataService.UpdateAsync(args));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id) => Ok(await _groupDataService.DeleteAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _groupDataService.GetOptionsAsync(selectOptions));
}
