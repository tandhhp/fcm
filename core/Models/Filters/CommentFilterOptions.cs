using Waffle.Entities;

namespace Waffle.Models.Filters;

public class CommentFilterOptions : FilterOptions
{
    public Guid? CatalogId { get; set; }
    public CommentStatus? Status { get; set; }
}
