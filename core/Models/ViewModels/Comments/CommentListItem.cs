using Waffle.Entities;

namespace Waffle.Models.ViewModels.Comments;

public class CommentListItem : Comment
{
    public string? UserName { get; set; }
    public string? CatalogName { get; set; }
    public string Avatar { get; set; } = default!;
    public string? FullName { get; set; }
}
