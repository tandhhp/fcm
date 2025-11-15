using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers;

public class CampaignController(ICampaignService _campaignService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] CampaignFilter filter) => Ok(await _campaignService.ListAsync(filter));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CampaignCreateArgs args) => Ok(await _campaignService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CampaignUpdateArgs args) => Ok(await _campaignService.UpdateAsync(args));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id) => Ok(await _campaignService.DeleteAsync(id));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id) => Ok(await _campaignService.GetAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectFilterOptions filterOptions) => Ok(await _campaignService.OptionsAsync(filterOptions));
}
