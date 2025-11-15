using Waffle.ExternalAPI.Models;

namespace Waffle.ExternalAPI.Interfaces
{
    public interface IFacebookService
    {
        Task<List<Album>> GetAlbumsAsync(string pageId, string access_token);
        Task<FacebookSummary> GetSummaryAsync(string id, string access_token);
        Task<FacebookListResult<FacebookPhoto>> GetPhotosAsync(string? id, int limit, string? before, string? after, string access_token);
        Task<LongLivedUserAccessToken?> GetLongLivedUserAccessTokenAsync(string appId, string appSercet, string shortLiveToken);
        Task<LongLivedPageAccessToken?> GetLongLivedPageAccessTokenAsync(string appId, string longLivedUserAccessToken);
        Task<FacebookListResult<FacebookProduct>> GetProductsAsync(string id, string access_token, int limit);
        Task<string> GraphAPIExplorerAsync(string query);
    }
}
