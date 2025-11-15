using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Models
{
    public class EpicGamesFreeGamesPromotions
    {
        [JsonPropertyName("data")]
        public EpicGamesFreeGamesPromotionsData Data { get; set; } = new();
    }

    public class EpicGamesFreeGamesPromotionsData
    {
        public EpicGamesCatalog Catalog { get; set; } = new();
    }

    public class EpicGamesCatalog
    {
        [JsonPropertyName("searchStore")]
        public EpicGamesSearchStore SearchStore { get; set; } = new();
    }

    public class EpicGamesSearchStore
    {
        [JsonPropertyName("elements")]
        public List<EpicGamesElement> Elements { get; set; } = new();
    }

    public class EpicGamesElement
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("keyImages")]
        public List<EpicGamesKeyImage> EpicGamesKeyImages { get; set; } = new();
        [JsonPropertyName("urlSlug")]
        public string? UrlSlug { get; set; }
        [JsonPropertyName("price")]
        public EpicGamesPrice Price { get; set; } = new();

        [JsonIgnore]
        public string Thumbnail => EpicGamesKeyImages.First(x => x.Type == "Thumbnail").Url ?? string.Empty;
    }

    public class EpicGamesKeyImage
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class EpicGamesPrice
    {
        [JsonPropertyName("totalPrice")]
        public EpicGamesTotalPrice TotalPrice { get; set; } = new();
    }

    public class EpicGamesTotalPrice
    {
        [JsonPropertyName("fmtPrice")]
        public EpicGamesPriceFmt FmtPrice { get; set; } = new();
    }

    public class EpicGamesPriceFmt
    {
        [JsonPropertyName("originalPrice")]
        public string? OriginalPrice { get; set; }
    }

    public class EpicGamesProduct
    {
        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }
        [JsonPropertyName("_slug")]
        public string? Slug { get; set;}
        [JsonPropertyName("pages")]
        public List<EpicGamesPage> Pages { get; set; } = new();
        [JsonPropertyName("_images_")]
        public List<string> Images { get; set; } = new();
    }

    public class EpicGamesPage
    {
        [JsonPropertyName("data")]
        public EpicGamesProductData Data { get; set; } = new();
    }

    public class EpicGamesProductData
    {
        [JsonPropertyName("about")]
        public EpicGamesAbout About { get; set; } = new();
    }

    public class EpicGamesAbout
    {
        [JsonPropertyName("shortDescription")]
        public string? ShortDescription { get; set; }
        [JsonPropertyName("image")]
        public EpicGamesImage Image { get; set; } = new();
    }

    public class EpicGamesImage
    {
        [JsonPropertyName("src")]
        public string? Src { get; set; }
    }
}
