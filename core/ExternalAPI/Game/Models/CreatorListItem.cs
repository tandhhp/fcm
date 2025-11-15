namespace Waffle.ExternalAPI.Game.Models
{
    public class CreatorListItem
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
    }

    public class CreatorListItemResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
    }
}
