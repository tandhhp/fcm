using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Leads.Args;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Entities;
using Waffle.Foundations;

namespace Waffle.Controllers;

public class LeadController(ILeadService _leadService) : BaseController
{
    [HttpGet("status-options")]
    public IActionResult GetStatusOptions()
    {
        var result = Enum.GetValues<LeadStatus>().Select(x => new
        {
            Value = x,
            Label = EnumHelper.GetDisplayName(x)
        });
        return Ok(result);
    }

    [HttpPost("reject")]
    public async Task<IActionResult> RejectAsync([FromBody] LeadRejectArgs args) => Ok(await _leadService.RejectAsync(args));

    [HttpPost("checkin")]
    public async Task<IActionResult> CheckinAsync([FromBody] LeadCheckinArgs args) => Ok(await _leadService.CheckinAsync(args));

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _leadService.DetailAsync(id));

    [HttpGet("waiting-list")]
    public async Task<IActionResult> GetWaitingListAsync([FromQuery] LeadWaittingListFilterOptions filterOptions) => Ok(await _leadService.GetWaitingListAsync(filterOptions));

    [HttpGet("checkin-list")]
    public async Task<IActionResult> GetCheckinListAsync([FromQuery] LeadCheckinListFilterOptions filterOptions) => Ok(await _leadService.GetCheckinListAsync(filterOptions));

    [HttpGet("export-checkin")]
    public async Task<IActionResult> ExportCheckinAsync([FromQuery] LeadCheckinListFilterOptions filterOptions)
    {
        var result = await _leadService.ExportCheckinAsync(filterOptions);
        if (!result.Succeeded) return BadRequest(result.Message);
        if (result.Data == null) return NotFound();
        return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "leads.xlsx");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] LeadUpdateArgs args) => Ok(await _leadService.UpdateAsync(args));

    [HttpPut("update-feedback")]
    public async Task<IActionResult> UpdateFeedbackAsync([FromBody] LeadUpdateFeedbackArgs args) => Ok(await _leadService.UpdateFeedbackAsync(args));

    [HttpPost("allowed-duplicate/{id}")]
    public async Task<IActionResult> AllowedDuplicateAsync([FromRoute] Guid id) => Ok(await _leadService.AllowedDuplicateAsync(id));
}
