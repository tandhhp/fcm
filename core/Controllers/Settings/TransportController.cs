using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Foundations;

namespace Waffle.Controllers.Settings;

public class TransportController(ITransportService _transportService) : BaseController
{
    [HttpGet("options")]
    public async Task<IActionResult> GetTransportOptions() => Ok(await _transportService.GetTransportOptionsAsync());
}
