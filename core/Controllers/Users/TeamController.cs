using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Teams.Args;
using Waffle.Core.Services.Teams.Models;
using Waffle.Foundations;

namespace Waffle.Controllers.Users;

public class TeamController(ITeamService _teamService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] TeamFilterOptions filterOptions) => Ok(await _teamService.ListAsync(filterOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateTeamArgs args) => Ok(await _teamService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateTeamArgs args) => Ok(await _teamService.UpdateAsync(args));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id) => Ok(await _teamService.DeleteAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] TeamSelectOptions selectOptions) => Ok(await _teamService.OptionsAsync(selectOptions));

    [HttpGet("users")]
    public async Task<IActionResult> UsersAsync([FromQuery] UserTeamFilterOptions filterOptions) => Ok(await _teamService.UsersAsync(filterOptions));

    [HttpPost("user")]
    public async Task<IActionResult> AddUserAsync([FromBody] TeamUserAddArgs args) => Ok(await _teamService.AddUserAsync(args));

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> RemoveUserAsync(Guid id) => Ok(await _teamService.RemoveUserAsync(id));

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] int id) => Ok(await _teamService.DetailAsync(id));
}