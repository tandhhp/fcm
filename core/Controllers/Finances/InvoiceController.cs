using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Finances.Invoices.Args;
using Waffle.Core.Services.Finances.Invoices.Filters;
using Waffle.Foundations;

namespace Waffle.Controllers.Finances;

public class InvoiceController(IInvoiceService _invoiceService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] InvoiceFilterOptions filterOptions) => Ok(await _invoiceService.ListAsync(filterOptions));

    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveAsync([FromRoute] Guid id) => Ok(await _invoiceService.ApproveAsync(id));

    [HttpPost("reject")]
    public async Task<IActionResult> RejectAsync([FromBody] InvoiceRejectArgs args) => Ok(await _invoiceService.RejectAsync(args));

    [HttpPost("cancel")]
    public async Task<IActionResult> CancelAsync([FromBody] InvoiceCancelArgs args) => Ok(await _invoiceService.CancelAsync(args));

    [HttpPost("export")]
    public async Task<IActionResult> ExportAsync([FromBody] InvoiceExportFilterOptions args)
    {
        var result = await _invoiceService.ExportAsync(args);
        if (!result.Succeeded) return BadRequest(result.Message);
        if (result.Data is null || result.Data.Length == 0) return BadRequest("Không có dữ liệu để xuất!");
        return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"invoice-{DateTime.Now:yyyyMMdd}.xlsx");
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> StatisticsAsync() => Ok(await _invoiceService.StatisticsAsync());

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] InvoiceUpdateArgs args) => Ok(await _invoiceService.UpdateAsync(args));

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _invoiceService.DetailAsync(id));
}
