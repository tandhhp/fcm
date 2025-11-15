using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService.Events;
using Waffle.Core.Services.Rooms.Models;
using Waffle.Core.Services.Tables.Models;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers;

public class RoomController(IRoomService _roomService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> GetListAsync([FromQuery] RoomFilterOptions filterOptions) => Ok(await _roomService.GetListAsync(filterOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] RoomCreateArgs args) => Ok(await _roomService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] RoomUpdateArgs args) => Ok(await _roomService.UpdateAsync(args));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id) => Ok(await _roomService.DeleteAsync(id));

    [HttpGet("tables")]
    public async Task<IActionResult> GetTablesAsync([FromQuery] TableFilterOptions filterOptions) => Ok(await _roomService.GetTablesAsync(filterOptions));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var data = await _roomService.FindAsync(id);
        if (data is null) return NotFound(new { Message = "Room not found!" });
        return Ok(TResult<object>.Ok(new
        {
            data.Name,
            data.Id,
            data.BranchId
        }));
    }
}