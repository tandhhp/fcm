using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Services.Attendances;
using Waffle.Core.Services.Attendances.Args;
using Waffle.Core.Services.Attendances.Filters;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Events;

public class AttendanceController(IAttendanceService _attendanceService) : BaseController
{
    [HttpGet("options")]
    public async Task<IActionResult> GetOptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _attendanceService.GetOptionsAsync(selectOptions));

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] AttendanceFilterOptions filterOptions) => Ok(await _attendanceService.ListAsync(filterOptions));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AttendanceCreateArgs args) => Ok(await _attendanceService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] AttendanceUpdateArgs args) => Ok(await _attendanceService.UpdateAsync(args));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id) => Ok(await _attendanceService.DeleteAsync(id));
}
