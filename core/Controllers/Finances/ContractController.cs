using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Contracts;
using Waffle.Core.Services.Contracts.Args;
using Waffle.Core.Services.Contracts.Filters;
using Waffle.Foundations;

namespace Waffle.Controllers.Finances;

public class ContractController(IContractService _contractService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] ContractFilterOptions filterOptions) => Ok(await _contractService.ListAsync(filterOptions));

    [HttpPost("payment")]
    public async Task<IActionResult> CreatePaymentAsync([FromBody] ContractCreatePayment args) => Ok(await _contractService.CreatePaymentAsync(args));

    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoicesAsync([FromQuery] ContractInvoiceFilterOptions filterOptions) => Ok(await _contractService.GetInvoicesAsync(filterOptions));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _contractService.DetailAsync(id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _contractService.DeleteAsync(id));

    [HttpGet("export")]
    public async Task<IActionResult> ExportAsync([FromQuery] ContractFilterOptions filterOptions)
    {
        var result = await _contractService.ExportAsync(filterOptions);
        if (!result.Succeeded) return BadRequest(result.Message);
        if (result.Data == null) return NotFound();
        return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "contracts.xlsx");
    }

    [HttpPost("add-gift")]
    public async Task<IActionResult> GiftContractAsync([FromBody] ContractGiftArgs args) => Ok(await _contractService.GiftContractAsync(args));

    [HttpPost("delete-gift")]
    public async Task<IActionResult> DeleteGiftContractAsync([FromBody] ContractGiftArgs args) => Ok(await _contractService.DeleteGiftContractAsync(args));

    [HttpGet("gifts")]
    public async Task<IActionResult> GetGiftsAsync([FromQuery] ContractGiftFilterOptions filterOptions) => Ok(await _contractService.GetGiftsAsync(filterOptions));

    [HttpGet("lead-options")]
    public async Task<IActionResult> GetLeadOptionsAsync([FromQuery] ContactLeadSelectOptions selectOptions) => Ok(await _contractService.GetLeadOptionsAsync(selectOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ContractCreateArgs args) => Ok(await _contractService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ContractUpdateArgs args) => Ok(await _contractService.UpdateAsync(args));
}
