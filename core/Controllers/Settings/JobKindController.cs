using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Services.JobKinds;
using Waffle.Core.Services.JobKinds.Models;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Settings;

[Route("api/job-kind")]
public class JobKindController(IJobKindService _jobKindService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] FilterOptions filterOptions) => Ok(await _jobKindService.ListAsync(filterOptions)); 

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) => Ok(await _jobKindService.GetAsync(id));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync() => Ok(await _jobKindService.OptionsAsync());

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateJobKindArgs args) => Ok(await _jobKindService.CreateAsync(args));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateJobKindArgs args) => Ok(await _jobKindService.UpdateAsync(args));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id) => Ok(await _jobKindService.DeleteAsync(id));
}
