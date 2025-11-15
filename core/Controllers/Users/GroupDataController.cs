using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Users;

[Route("api/group-data")]
public class GroupDataController(IGroupDataService _groupDataService) : BaseController
{
    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _groupDataService.GetOptionsAsync(selectOptions));
}
