using Waffle.ExternalAPI.Models;

namespace Waffle.ExternalAPI.Interfaces
{
    public interface IShopeeService
    {
        Task<BaseInfoAndLinks> GetBaseInfoAndLinksAsync(int pageNum);
        Task<LandingPageLinkList> GetLinkListAsync(string? urlSuffix, string? groupId, string? searchTerm);
        Task<LandingPageLinkList> GetLinkListsAsync(string tag, int pageNum, int pageSize);
    }
}
