using System.Text.Json.Serialization;
using Waffle.Entities;

namespace Waffle.Models.Components
{
    public class Navbar : BaseEntity
    {
        [JsonPropertyName("container")]
        public bool Container { get; set; }
        [JsonPropertyName("layout")]
        public Layout Layout { get; set; }
        [JsonIgnore]
        public List<NavItem> NavItems { get; set; } = new List<NavItem>();
    }

    public class NavItem : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("links")]
        public List<Link> Links { get; set; } = new List<Link>();
        [JsonIgnore]
        public bool HasSubItem => Links.Count > 1;
    }

    public enum Layout
    {
        Default,
        Vertical
    }
}
