using Waffle.Core.Foundations.Interfaces;

namespace Waffle.Models;

public class SelectOptions : IILocale
{
    public string? KeyWords { get; set; }
    public string Locale { get; set; } = "vi-VN";
}
