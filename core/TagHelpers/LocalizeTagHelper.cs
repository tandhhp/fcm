using Microsoft.AspNetCore.Razor.TagHelpers;
using Waffle.Core.Interfaces.IService;

namespace Waffle.TagHelpers;

public class LocalizeTagHelper : TagHelper
{
    private readonly ILocalizationService _localizationService;
    public LocalizeTagHelper(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string? Key { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Key))
        {
            return;
        }
        output.TagName = null;
        output.Content.SetContent(await _localizationService.GetAsync(Key));
    }
}
