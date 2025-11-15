using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Finances.Bills.Args;
using Waffle.Core.Services.Finances.Bills.Filters;
using Waffle.Core.Services.Finances.Interfaces;
using Waffle.Foundations;

namespace Waffle.Controllers.Finances;

public class BillController(IBillService _billService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] BillFilterOptions filterOptions) => Ok(await _billService.ListAsync(filterOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] BillCreateArgs args) => Ok(await _billService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] BillUpdateArgs args) => Ok(await _billService.UpdateAsync(args));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _billService.DeleteAsync(id));

    [HttpPost("approve/{id:guid}")]
    public async Task<IActionResult> ApproveAsync([FromRoute] Guid id) => Ok(await _billService.ApproveAsync(id));

    [HttpPost("reject/{id:guid}")]
    public async Task<IActionResult> RejectAsync([FromRoute] Guid id) => Ok(await _billService.RejectAsync(id));
}
