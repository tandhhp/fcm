using Waffle.ExternalAPI.Googles.Models;
using Waffle.ExternalAPI.Models.GoogleAggregate;

namespace Waffle.ExternalAPI.Interfaces;

public interface IGoogleService
{
    Task<Trend?> GetDailyTrendingAsync();
    Task<BloggerListResult<BloggerItem>?> BloggerSearchAsync(string blogId, string apiKey, string searchTerm);
    Task<BloggerListResult<BloggerItem>?> BloggerPostsAsync(string? blogId, string? apiKey, int maxResults, string pageToken, string? labels);
    Task<BloggerItem?> BloggerGetAsync(string? blogId, string? postId, string? apiKey);
    Task<BloggerItem?> BloggerGetByPathAsync(string? blogId, string path, string? apiKey);
}
