using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService.Finances;
using Waffle.Core.Services.Finances.Counpons.Args;
using Waffle.Core.Services.Finances.Counpons.Filters;
using Waffle.Foundations;

namespace Waffle.Controllers.Finances;

public class CouponController(ICouponService _counponService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] CouponFilterOptions filterOptions) => Ok(await _counponService.ListAsync(filterOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CouponCreateArgs args) => Ok(await _counponService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CouponUpdateArgs args) => Ok(await _counponService.UpdateAsync(args));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _counponService.DeleteAsync(id));
}
