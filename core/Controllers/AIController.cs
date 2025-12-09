using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers;

public class AIController : BaseController
{
    private readonly IAIService _aiService;

    public AIController(IAIService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> ChatAsync([FromBody] ChatMessageRequest request)
    {
        if (request.Messages is null)
        {
            return BadRequest(TResult.Failed("Nội dung tin nhắn không được để trống"));
        }

        var result = await _aiService.ChatAsync(request.Messages);
        return Ok(result);
    }

    [HttpPost("chat-with-context")]
    public async Task<IActionResult> ChatWithContextAsync([FromBody] ChatWithContextRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest(TResult.Failed("Nội dung tin nhắn không được để trống"));
        }

        var result = await _aiService.ChatWithContextAsync(request.Content, request.Context);
        return Ok(result);
    }
}

public class ChatMessageRequest
{
    public List<ChatRequest>? Messages { get; set; }
}

public class ChatRequest
{
    public string Role { get; set; } = default!;
    public string Content { get; set; } = string.Empty;
}

public class ChatWithContextRequest
{
    public string Content { get; set; } = string.Empty;
    public string? Context { get; set; }
}