using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Events.Args;
using Waffle.Core.Services.Events.Filters;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers;

public class EventController(IEventService _eventService) : BaseController
{
    [HttpGet("list-sale-revenue")]
    public async Task<IActionResult> ListSaleRevenueAsync([FromQuery] SaleRevenueFilterOptions filterOptions) => Ok(await _eventService.ListSaleRevenueAsync(filterOptions));

    [HttpGet("list-key-in-revenue")]
    public async Task<IActionResult> ListKeyInRevenueAsync([FromQuery] SaleRevenueFilterOptions filterOptions) => Ok(await _eventService.ListKeyInRevenueAsync(filterOptions));

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] EventFilterOptions filterOptions) => Ok(await _eventService.ListAsync(filterOptions));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync() => Ok(await _eventService.OptionsAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _eventService.DetailAsync(id));

    [HttpGet("table-options")]
    public async Task<IActionResult> TableOptionsAsync([FromQuery] AllTableFilterOptions filterOptions) => Ok(await _eventService.TableOptionsAsync(filterOptions));

    [HttpPost("close-deal")]
    public async Task<IActionResult> CloseDealAsync([FromBody] CloseDealArgs args) => Ok(await _eventService.CloseDealAsync(args));

    [HttpGet("su-report")]
    public async Task<IActionResult> SuReportAsync([FromQuery] SUFilterOptions filterOptions) => Ok(await _eventService.SuReportAsync(filterOptions));

    [HttpGet("key-in-options")]
    public async Task<IActionResult> KeyInOptionsAsync([FromQuery] KeyInSelectOptions selectOptions) => Ok(await _eventService.KeyInOptionsAsync(selectOptions));

    [HttpGet("to-options")]
    public async Task<IActionResult> ToOptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _eventService.ToOptionsAsync(selectOptions));
}
