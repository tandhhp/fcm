using Waffle.Models;

namespace Waffle.ExternalAPI.Models.WordPress;

public class OpenWordPressApi : SearchFilterOptions
{
    public string? Domain { get; set; }
}
