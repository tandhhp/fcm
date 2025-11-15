using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Waffle.Entities;
using Waffle.Entities.Ecommerces;

namespace Waffle.Models
{
    public class BackupListItem
    {
        [JsonPropertyName("workContents")]
        public List<WorkContent> WorkContents { get; set; } = new();
        [JsonPropertyName("workItems")]
        public List<WorkItem> WorkItems { get; set; } = new();
        [JsonPropertyName("fileContents")]
        public List<FileContent> FileContents { get; set; } = new();
        [JsonPropertyName("appSettings")]
        public List<AppSetting> AppSettings { get; set; } = new();
        [JsonPropertyName("catalogs")]
        public List<Catalog> Catalogs { get; set; } = new();
        [JsonPropertyName("components")]
        public List<Component> Components { get; set; } = new();
        [JsonPropertyName("localizations")]
        public List<Localization> Localizations { get; set; } = new();
        [JsonPropertyName("users")]
        public List<ApplicationUser> Users { get; set; } = new();
        [JsonPropertyName("roles")]
        public List<ApplicationRole> Roles { get; set; } = new();
        [JsonPropertyName("userRoles")]
        public List<IdentityUserRole<Guid>> UserRoles { get; set; } = new();
        [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; } = new();
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();
        [JsonPropertyName("orders")]
        public List<Order> Orders { get; set; } = new();
        [JsonPropertyName("orderDetails")]
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
