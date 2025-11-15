using Waffle.Models.Components;

namespace Waffle.Core.Constants;

public class Themes
{
    public static List<Option> All => new()
    {
        new Option { Label = "Default", Value = "default" },
        new Option { Label = "DLiTi", Value = "dliti" },
        new Option { Label = "Shinec", Value = "shinec" }
    };
}
