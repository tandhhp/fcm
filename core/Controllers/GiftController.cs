using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Models;
using Waffle.Foundations;

namespace Waffle.Controllers;

public class GiftController(IGiftService _giftService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] GiftFilterOptions filterOptions) => Ok(await _giftService.ListAsync(filterOptions));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _giftService.DetailAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GiftCreateArgs args) => Ok(await _giftService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] GiftUpdateArgs args) => Ok(await _giftService.UpdateAsync(args));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _giftService.DeleteAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> GetOptionsAsync() => Ok(await _giftService.GetOptionsAsync());
}
