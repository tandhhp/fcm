namespace Waffle.Core.Options;

public class SettingOptions
{
    public const string Settings = "Settings";
    public string? DefaultLanguage { get; set; }
    public string Theme { get; set; } = default!;
}
