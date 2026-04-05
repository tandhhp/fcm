using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Data;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Teles;

[Route("api/call-center")]
public class CallCenterController(ICallCenterService _callCenterService, ApplicationDbContext _context) : BaseController
{
    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _callCenterService.GetOptionsAsync(selectOptions));

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] FilterOptions filterOptions)
    {
        var query = from cc in _context.CallCenters
                    select new
                    {
                        cc.Id,
                        cc.Name,
                        cc.Code,
                        cc.SortOrder,
                        TeamCount = _context.Teams.Count(t => t.CallCenterId == cc.Id)
                    };
        query = query.OrderBy(x => x.SortOrder);
        return Ok(await ListResult<object>.Success(query, filterOptions));
    }
}
