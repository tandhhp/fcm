using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Models;
using Waffle.Models.Filters;
using Waffle.Models.Params;
using Waffle.Models.ViewModels.Comments;

namespace Waffle.Core.Services;

public class CommentService(ICatalogRepository catalogRepository, IHCAService _hcaService, ILogService _logService, ICommentRepository _commentRepository, UserManager<ApplicationUser> userManager) : ICommentService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ICatalogRepository _catalogRepository = catalogRepository;

    public async Task<TResult> ActiveAsync(Guid id)
    {
        var comment = await _commentRepository.FindAsync(id);
        if (comment is null) return TResult.Failed("Không tìm thấy bình luận!");

        var user = await _userManager.FindByIdAsync(_hcaService.GetUserId().ToString());
        if (user is null) return TResult.Failed("Không tìm thấy người dùng!");
        await _logService.AddAsync($"{user.Name} đã phê duyệt bình luận {comment.Content}");

        comment.Status = CommentStatus.Active;
        await _commentRepository.UpdateAsync(comment);

        return TResult.Success;
    }

    public async Task<IdentityResult> AddAsync(AddComment addComment)
    {
        if (string.IsNullOrWhiteSpace(addComment.Message))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "dataEmpty",
                Description = "Message empty"
            });
        }
        if (addComment.ParrentId != null && !await AnyAsync(addComment.ParrentId ?? Guid.Empty))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "DataNotFound",
                Description = "Data not found!"
            });
        }
        var comment = new Comment
        {
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            UserId = _hcaService.GetUserId(),
            Status = CommentStatus.Draft,
            CatalogId = addComment.CatalogId,
            Content = addComment.Message,
            ParrentId = addComment.ParrentId,
            Rate = addComment.Rate
        };
        await _commentRepository.AddAsync(comment);
        return IdentityResult.Success;
    }

    public async Task<bool> AnyAsync(Guid id) => await _commentRepository.Queryable().AnyAsync(x => x.Id == id);

    public async Task<IdentityResult> DeleteAsync(Guid id)
    {
        var comment = await _commentRepository.FindAsync(id);
        if (comment is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.dataNotFound",
                Description = "Data not found!"
            });
        }

        await _commentRepository.DeleteAsync(comment);

        var user = await _userManager.FindByIdAsync(_hcaService.GetUserId().ToString());
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.userNotFound",
                Description = "User not found!"
            });
        }

        var catalog = await _catalogRepository.FindAsync(comment.CatalogId);
        if (catalog is null)
        {
            return IdentityResult.Success;
        }
        await _logService.AddAsync($"{user.Name} đã xóa bình luận {comment.Content} về {catalog.Name}");

        return IdentityResult.Success;
    }

    public async Task<ListResult<CommentListItem>> ListAsync(CommentFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var admin = await _userManager.FindByIdAsync(userId.ToString());
        if (admin is null)
        {
            return ListResult<CommentListItem>.Failed("User not found");
        }

        var query = from comment in _commentRepository.Queryable()
                    join user in _userManager.Users on comment.UserId equals user.Id
                    join catalog in _catalogRepository.Queryable() on comment.CatalogId equals catalog.Id
                    where (filterOptions.Status == null || comment.Status == filterOptions.Status)
                    && (filterOptions.CatalogId == null || comment.CatalogId == filterOptions.CatalogId)
                    select new CommentListItem
                    {
                        CatalogId = comment.CatalogId,
                        UserId = user.Id,
                        Content = comment.Content,
                        CreatedDate = comment.CreatedDate,
                        Id = comment.Id,
                        ModifiedDate = comment.ModifiedDate,
                        ParrentId = comment.ParrentId,
                        Status = comment.Status,
                        UserName = user.UserName,
                        CatalogName = catalog.Name,
                        FullName = user.Name,
                        Avatar = user.Avatar ?? "https://nuras.com.vn/imgs/icons/noavatar.jpg",
                        Rate = comment.Rate
                    };
        if (await _userManager.IsInRoleAsync(admin, RoleName.CardHolder))
        {
            query = query.Where(x => x.UserId == userId || x.Status != CommentStatus.Draft);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<CommentListItem>.Success(query, filterOptions);
    }

    public Task<ListResult<CommentListItem>> ListInCatalogAsync(CommentFilterOptions filterOptions) => _commentRepository.ListInCatalogAsync(filterOptions);

    public async Task<IdentityResult> RemoveAsync(Guid id)
    {
        var comment = await _commentRepository.FindAsync(id);
        if (comment is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.dataNotFound",
                Description = "Data not found!"
            });
        }

        await _commentRepository.DeleteAsync(comment);

        var user = await _userManager.FindByIdAsync(_hcaService.GetUserId().ToString());
        if (user is null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "error.userNotFound",
                Description = "User not found!"
            });
        }

        var catalog = await _catalogRepository.FindAsync(comment.CatalogId);
        if (catalog is null)
        {
            return IdentityResult.Success;
        }
        await _logService.AddAsync($"{user.Name} đã xóa bình luận {comment.Content} về {catalog.Name}");

        return IdentityResult.Success;
    }
}
