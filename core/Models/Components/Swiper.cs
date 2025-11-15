using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Swiper", ShortName = "SWIPER")]
public class Swiper : AbstractComponent
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("mode")]
    public string Mode { get; set; } = "default";
    [JsonPropertyName("slidesPerView")]
    public int SlidesPerView { get; set; }

    [JsonIgnore]
    public List<SwiperItem> Items { get; set; } = new();
}

[Display(Name = "Swiper Item")]
public class SwiperItem
{
    public string? Title { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public bool IsVideo { get; set; }
    public HeroType Type { get; set; }
    public string? Voucher { get; set; }
}

public enum HeroType
{
    Default,
    Tour,
    Healthcare
}