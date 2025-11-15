using System.Text.Json;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Models;

namespace Waffle.ExternalAPI.Services;

public class WordPressService : IWordPressService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WordPressService> _logger;

    public WordPressService(HttpClient httpClient, ILogger<WordPressService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<WordPressPost?> GetPostAsync(string domain, string? id)
    {
        if (string.IsNullOrEmpty(id)) return default;
        try
        {
            var uri = new Uri(domain);
            _httpClient.BaseAddress = uri;
            var url = $"wp-json/wp/v2/posts/{id}";
            var response = await _httpClient.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<WordPressPost>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception {Message}", ex.Message);
            return default;
        }
    }

    public async Task<IEnumerable<WordPressPost>?> ListPostAsync(string domain, SearchFilterOptions filterOptions)
    {
        try
        {
            var uri = new Uri(domain);
            _httpClient.BaseAddress = uri;
            var url = $"wp-json/wp/v2/posts?page={filterOptions.Current}&search={filterOptions.SearchTerm}&_embed&per_page={filterOptions.PageSize}";
            var response = await _httpClient.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<List<WordPressPost>>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception {Message}", ex.Message);
            return default;
        }
    }
}
