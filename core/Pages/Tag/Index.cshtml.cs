using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Models;
using Waffle.Models.Components;
using Waffle.Models.ViewModels;

namespace Waffle.Pages.Tag;

public class IndexModel : EntryPageModel
{
    public IndexModel(ICatalogService catalogService) : base(catalogService) { }

    public ListGroup Tags = new();

    [BindProperty(SupportsGet = true)]
    public int Current { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    [UIHint(UIHint.SearchBox)]
    public string? SearchTerm { get; set; }
    [UIHint(UIHint.Pagination)]

    public Pagination Pagination = new();

    public async Task OnGetAsync()
    {
        var tags = await _catalogService.ListTagAsync(new TagFilterOptions
        {
            Current = Current,
            PageSize = 20,
            KeyWords = SearchTerm
        });
        Tags = new ListGroup
        {
            Name = PageData.Name,
            Items = GetItems(tags.Data ?? new List<TagListItem>()),
            HasBadge = true
        };
        Pagination = tags.Pagination;
    }

    private static List<ListGroupItem> GetItems(IEnumerable<TagListItem> tags)
    {
        var returnValue = new List<ListGroupItem>();
        foreach (var tag in tags)
        {
            returnValue.Add(new ListGroupItem
            {
                Link = new Link
                {
                    Href = $"/tag/{tag.NormalizedName}",
                    Name = tag.Name,
                },
                Badge = tag.PostCount,
                Suffix = $"<span class=\"text-muted text-sm\"> - {tag.ViewCount} <i class=\"fas fa-eye\"></i></span>"
            });
        }
        return returnValue;
    }
}
