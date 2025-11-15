using Waffle.ExternalAPI.Game.Models;
using Waffle.ExternalAPI.Models;
using Waffle.Models;

namespace Waffle.ExternalAPI.Interfaces
{
    public interface IGameService
    {
        Task<string> LoL_GetVersionAsync();
        Task<List<string>> LOL_GetLanguagesAsync();
        Task<IDictionary<string, LoL_Champion>> GetChampionsAsync(string version, string lang);
        Task<List<EpicGamesElement>> GetEpicGamesFreeGamesPromotionsAsync();
        Task<EpicGamesProduct?> GetEpicGamesProductAsync(string normalizedName);
        Task<CreatorListItem> GetCreatorsAsync(int pageIndex, int pageSize);

        Task<GIContentList> GetGIContentListAsync(BasicFilterOptions filterOptions);
        Task<GIContent?> GetGIContentDetailAsync(int id);
    }
}
