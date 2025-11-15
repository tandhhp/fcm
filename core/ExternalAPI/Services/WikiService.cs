using System.Text.Json;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Action = Waffle.ExternalAPI.Models.Action;

namespace Waffle.ExternalAPI.Services
{
    public class WikiService : IWikiService
    {
        private readonly HttpClient _http;
        private readonly ILogger<WikiService> _logger;
        public WikiService(HttpClient http, ILogger<WikiService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<Parse> FandomAsync(string page, string name, string lang)
        {
            try
            {
                var url = $"https://{name}.fandom.com/{lang}/api.php?format=json&formatversion=2";
                var response = await _http.GetStreamAsync($"{url}&action=query&prop=text&page={page}");
                var data = await JsonSerializer.DeserializeAsync<Action>(response);
                return data?.Parse ?? new Parse();
            }
            catch (Exception)
            {
                return new Parse();
            }
        }

        public async Task<WikiQuery> GetLangLinksAsync(string titles)
        {
            try
            {
                var url = $"https://wikipedia.org/w/api.php?action=query&format=json&formatversion=2&prop=langlinks&titles={titles}";
                var response = await _http.GetStreamAsync(url);
                var data = await JsonSerializer.DeserializeAsync<WikiQueryResult>(response);
                return data?.Query ?? new WikiQuery();
            }
            catch (Exception)
            {
                return new WikiQuery();
            }
        }

        public async Task<Parse?> ParseAsync(string page, string lang)
        {
            try
            {
                var url = $"https://{lang}.wikipedia.org/w/api.php?format=json&formatversion=2";
                var response = await _http.GetStreamAsync($"{url}&action=parse&prop=text&page={page}");
                var data = await JsonSerializer.DeserializeAsync<Action>(response);
                return data?.Parse;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Wiki Exception");
                return default;
            }
        }
    }
}
