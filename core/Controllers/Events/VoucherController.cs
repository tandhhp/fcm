using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Vouchers.Args;
using Waffle.Foundations;

namespace Waffle.Controllers.Events;

public class VoucherController(IVoucherService _voucherService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] VoucherFilterOptions filterOptions) => Ok(await _voucherService.ListAsync(filterOptions));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _voucherService.DetailAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] VoucherCreateArgs args) => Ok(await _voucherService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] VoucherUpdateArgs args) => Ok(await _voucherService.UpdateAsync(args));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _voucherService.DeleteAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> GetOptionsAsync([FromQuery] VoucherSelectOptions selectOptions) => Ok(await _voucherService.GetOptionsAsync(selectOptions));

    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync([FromForm] VoucherImportArgs args) => Ok(await _voucherService.ImportAsync(args));

    [HttpGet("export"), AllowAnonymous]
    public async Task<IActionResult> ExportAsync([FromQuery] VoucherFilterOptions filterOptions)
    {
        var result = await _voucherService.ExportAsync(filterOptions);
        if (!result.Succeeded) return BadRequest(result.Message);
        if (result.Data == null) return NotFound();
        return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "vouchers.xlsx");
    }
}
