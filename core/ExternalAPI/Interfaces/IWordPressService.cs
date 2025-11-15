using Waffle.ExternalAPI.Models;
using Waffle.Models;

namespace Waffle.ExternalAPI.Interfaces;

public interface IWordPressService
{
    Task<WordPressPost?> GetPostAsync(string domain, string? postId);
    Task<IEnumerable<WordPressPost>?> ListPostAsync(string domain, SearchFilterOptions filterOptions);
}
