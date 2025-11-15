using Microsoft.AspNetCore.Identity;
using Waffle.Models;
using Waffle.Models.Filters;
using Waffle.Models.Params;
using Waffle.Models.ViewModels.Comments;

namespace Waffle.Core.Interfaces.IService;

public interface ICommentService
{
    Task<IdentityResult> AddAsync(AddComment addComment);
    Task<bool> AnyAsync(Guid id);
    Task<IdentityResult> DeleteAsync(Guid id);
    Task<IdentityResult> RemoveAsync(Guid id);
    Task<ListResult<CommentListItem>> ListAsync(CommentFilterOptions filterOptions);
    Task<ListResult<CommentListItem>> ListInCatalogAsync(CommentFilterOptions filterOptions);
    Task<TResult> ActiveAsync(Guid id);
}
