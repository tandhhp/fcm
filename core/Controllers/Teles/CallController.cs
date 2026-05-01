using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService.Calls;
using Waffle.Core.Services.Calls.Args;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.ExternalAPI.Tel4vn;
using Waffle.ExternalAPI.Tel4vn.Filters;
using Waffle.Foundations;

namespace Waffle.Controllers.Teles;

public class CallController(ICallStatusService _callStatusService, ICallHistoryService _callHistoryService, ITel4vnService _tel4VnService) : BaseController
{
    [HttpGet("status/options")]
    public async Task<IActionResult> StatusOptionsAsync([FromQuery] CallStatusSelectOptions options) => Ok(await _callStatusService.OptionsAsync(options));

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteAsync([FromBody] CallCompleteArgs args) => Ok(await _callHistoryService.CompleteAsync(args));

    [HttpGet("histories")]
    public async Task<IActionResult> HistoriesAsync([FromQuery] CallHistoryFilterOptions filterOptions) => Ok(await _callHistoryService.HistoriesAsync(filterOptions));

    [HttpGet("statistics")]
    public async Task<IActionResult> StatisticsAsync() => Ok(await _callHistoryService.StatisticsAsync());

    [HttpGet("status/list")]
    public async Task<IActionResult> StatusListAsync([FromQuery] CallStatusFilterOptions filterOptions) => Ok(await _callStatusService.ListAsync(filterOptions));

    [HttpPost("status")]
    public async Task<IActionResult> CreateStatusAsync([FromBody] CallStatusCreateArgs args) => Ok(await _callStatusService.CreateAsync(args));

    [HttpDelete("status/{id:int}")]
    public async Task<IActionResult> DeleteStatusAsync([FromRoute] int id) => Ok(await _callStatusService.DeleteAsync(id));

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatusAsync([FromBody] CallStatusUpdateArgs args) => Ok(await _callStatusService.UpdateAsync(args));

    [HttpGet("tele-report")]
    public async Task<IActionResult> TeleRepostAsync([FromQuery] TeleReportFilterOptions filterOptions) => Ok(await _callHistoryService.TeleReportAsync(filterOptions));

    [HttpGet("cdr"), AllowAnonymous]
    public async Task<IActionResult> GetCdrAsync([FromQuery] CdrFilterOptions filterOptions) => Ok(await _tel4VnService.GetCdrAsync(filterOptions));

    [HttpPost("cdr/webhook"), AllowAnonymous]
    public async Task<IActionResult> CdrWebhookAsync([FromBody] CdrWebhookCreateArgs args) => Ok(await _callHistoryService.CdrWebhookAsync(args));

    [HttpGet("webhook-logs")]
    public async Task<IActionResult> WebhookLogsAsync([FromQuery] CallWebhookLogFilterOptions filterOptions) => Ok(await _callHistoryService.WebhookLogsAsync(filterOptions));

    [HttpGet("webhook-logs/export")]
    public async Task<IActionResult> ExportWebhookLogsAsync([FromQuery] CallWebhookLogFilterOptions filterOptions)
    {
        var result = await _callHistoryService.ExportWebhookLogsAsync(filterOptions);
        if (!result.Succeeded) return BadRequest(result.Message);
        if (result.Data == null) return NotFound();
        return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"call_webhook_logs_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    [HttpGet("status-details")]
    public async Task<IActionResult> GetStatusDetailsAsync([FromQuery] CallStatusDetailFilterOptions filterOptions) => Ok(await _callHistoryService.GetStatusDetailsAsync(filterOptions));
}
