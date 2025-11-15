using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Pages.Leaf.Categories;

public class DetailModel : DynamicPageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IComponentService _componentService;
    private readonly IWordPressService _wordPressService;
    private readonly IWorkService _workService;

    public DetailModel(ICatalogService catalogService, ApplicationDbContext context, IComponentService componentService, IWordPressService wordPressService, IWorkService workService) : base(catalogService)
    {
        _context = context;
        _componentService = componentService;
        _wordPressService = wordPressService;
        _workService = workService;

    }

    public List<WorkListItem> Works = new();
    [UIHint(UIHint.Tags)]
    public List<Catalog> Tags = new();
    public bool HasTag => Tags.Any();
    public LandingPageLinkList ShopeeProducts = new();

    public string Email = string.Empty;
    public bool IsAuthenticated = false;
    public string? PostContent;
    public IEnumerable<ComponentListItem>? Components;

    public async Task<IActionResult> OnGetAsync()
    {
        if (Category != null)
        {
            if (Category.Type == CatalogType.WordPress)
            {
                var component = await _componentService.EnsureComponentAsync(nameof(WordPressLister));
                var query = from a in _context.WorkItems
                            join b in _context.WorkContents on a.WorkId equals b.Id
                            where b.ComponentId == component.Id && a.CatalogId == Category.Id
                            select b;
                var work = await query.FirstOrDefaultAsync();
                if (work is null || string.IsNullOrEmpty(work.Arguments)) return NotFound();
                var wordPressLister = _workService.Get<WordPressLister>(work.Arguments);

                if (wordPressLister is null) return NotFound();
                var postId = PageData.NormalizedName.Split("-").LastOrDefault();

                var post = await _wordPressService.GetPostAsync(wordPressLister.Domain, postId);
                if (post is null) return NotFound();
                string re = @"<a [^>]+>(.*?)<\/a>";
                PostContent = Regex.Replace(post.Content.Rendered ?? string.Empty, re, string.Empty);
                PageData.Name = post.Title.Rendered ?? string.Empty;
                PageData.ModifiedDate = post.Date ?? DateTime.Now;
                PageData.ModifiedDate = post.Date ?? DateTime.Now;
                ViewData["Title"] = PageData.Name;
                return Page();
            }
            Components = await _catalogService.ListComponentAsync(PageData.Id);
        }
        Tags = await _catalogService.ListTagByIdAsync(PageData.Id);
        IsAuthenticated = User.Identity?.IsAuthenticated ?? false;
        return Page();
    }
}
