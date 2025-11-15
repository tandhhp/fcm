using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Filters;
using Waffle.Models.ViewModels.Comments;

namespace Waffle.Core.Interfaces.IRepository;

public interface ICommentRepository : IAsyncRepository<Comment>
{
    Task<ListResult<CommentListItem>> ListInCatalogAsync(CommentFilterOptions filterOptions);
}
