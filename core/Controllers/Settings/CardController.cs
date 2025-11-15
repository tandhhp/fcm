using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Cards;
using Waffle.Core.Services.Cards.Args;
using Waffle.Core.Services.Cards.Filters;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Settings;

public class CardController(ICardService _cardService) : BaseController
{
    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _cardService.OptionsAsync(selectOptions));

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] CardFilterOptions filterOptions) => Ok(await _cardService.ListAsync(filterOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CardCreateArgs args) => Ok(await _cardService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CardUpdateArgs args) => Ok(await _cardService.UpdateAsync(args));

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailAsync([FromRoute] Guid id) => Ok(await _cardService.DetailAsync(id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _cardService.DeleteAsync(id));
}
