using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Interfaces.IService;
using Waffle.Entities;
using Waffle.Extensions;
using Waffle.Models.Components;
using Waffle.Models.Filters;

namespace Waffle.ViewComponents;

public class CommentViewComponent : ViewComponent
{
    private readonly ICommentService _commentService;
    public CommentViewComponent(ICommentService commentService)
    {
        _commentService = commentService;
    }

    protected Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var work = new CommentComponent
        {
            Comments = await _commentService.ListInCatalogAsync(new CommentFilterOptions
            {
                CatalogId = PageData.Id
            }),
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            CurrentUrl = PageData.GetUrl(),
            CatalogId = PageData.Id
        };
        return View(work);
    }
}
