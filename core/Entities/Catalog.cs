using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Waffle.Entities;

public class Catalog : BaseEntity
{
    [JsonPropertyName("parentId")]
    public Guid? ParentId { get; set; }
    [StringLength(200)]
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [StringLength(500)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [StringLength(200), Required]
    [JsonPropertyName("normalizedName")]
    public string NormalizedName { get; set; } = default!;
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }
    [JsonPropertyName("type")]
    public CatalogType Type { get; set; }
    [JsonPropertyName("viewCount")]
    public int ViewCount { get; set; }
    [JsonPropertyName("thumbnail")]
    [StringLength(250)]
    public string? Thumbnail { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
    [JsonPropertyName("createdBy")]
    public Guid? CreatedBy { get; set; }
    [JsonPropertyName("locale")]
    [StringLength(10)]
    public string? Locale { get; set; }
}

public enum CatalogType
{
    Default = 0,
    Article = 1,
    Product = 2,
    Setting = 3,
    Entry = 4,
    Location = 5,
    Album = 8,
    Tag = 9,
    Video = 10,
    Game = 11,
    WordPress = 12,
    Brand = 13,
    Tour = 14,
    Healthcare = 15,
    Hospital = 16,
    [Display(Name = "Khách sạn lẻ")]
    Room = 17,
    /// <summary>
    /// Những sản phẩm dịch vụ không đăng ký được bởi khách
    /// </summary>
    [Display(Name = "Đặc biệt")]
    Special
}
