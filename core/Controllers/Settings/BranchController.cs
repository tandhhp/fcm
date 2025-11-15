using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Branches;

namespace Waffle.Controllers.Settings;

public class BranchController(IBranchService _branchService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] BranchFilterOptions filterOptions) => Ok(await _branchService.ListAsync(filterOptions));

    [HttpGet("options")]
    public async Task<IActionResult> OptionsAsync([FromQuery] SelectOptions selectOptions) => Ok(await _branchService.OptionsAsync(selectOptions));
}
