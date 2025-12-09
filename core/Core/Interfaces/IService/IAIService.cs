using Waffle.Controllers;
using Waffle.Models;

namespace Waffle.Core.Interfaces.IService;

public interface IAIService
{
    Task<TResult<string>> ChatAsync(List<ChatRequest> requests);
    Task<TResult<string>> ChatWithContextAsync(string content, string? context);
}