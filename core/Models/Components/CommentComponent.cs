using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;
using Waffle.Models.ViewModels.Comments;

namespace Waffle.Models.Components;

[Display(Name = "Comment", Prompt = "comment", AutoGenerateField = true)]
public class CommentComponent : AbstractComponent
{
    [JsonIgnore]
    public ListResult<CommentListItem> Comments { get; set; } = new();
    [JsonIgnore]
    public bool IsAuthenticated { get; set; }
    [JsonIgnore]
    public string? CurrentUrl { get; set; }
    [JsonIgnore]
    public Guid CatalogId { get; set; }
}
