using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Foundations;
using Waffle.Models.Filters;
using Waffle.Models.Params;

namespace Waffle.Controllers;

public class CommentController(ICommentService _commentService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] CommentFilterOptions filterOptions) => Ok(await _commentService.ListAsync(filterOptions));

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] AddComment addComment) => Ok(await _commentService.AddAsync(addComment));

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) => Ok(await _commentService.DeleteAsync(id));

    [HttpPost("remove/{id}")]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid id) => Ok(await _commentService.RemoveAsync(id));

    [HttpPost("active/{id}")]
    public async Task<IActionResult> ActiveAsync([FromRoute] Guid id) => Ok(await _commentService.ActiveAsync(id));
}
