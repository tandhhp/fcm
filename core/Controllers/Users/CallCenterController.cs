using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Users;

[Route("api/call-center")]
public class CallCenterController(ICallCenterService _callCenterService) : BaseController
{
    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _callCenterService.GetOptionsAsync(selectOptions));
}
