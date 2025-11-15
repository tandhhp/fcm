using Waffle.ExternalAPI.Models;

namespace Waffle.ExternalAPI.Interfaces
{
    public interface IWikiService
    {
        Task<Parse?> ParseAsync(string page, string lang);
        Task<Parse> FandomAsync(string page, string name, string lang);
        Task<WikiQuery> GetLangLinksAsync(string titles);
    }
}
