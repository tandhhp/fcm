using Microsoft.AspNetCore.Mvc;
using Waffle.Models.Components;
using Waffle.Models;
using Waffle.Core.Interfaces.IService;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Entities;
using Waffle.ExternalAPI.Models;

namespace Waffle.ViewComponents;

public class WordPressListerViewComponent : ViewComponent
{
    private readonly IWorkService _workService;
    private readonly IWordPressService _wordPressService;

    public WordPressListerViewComponent(IWorkService workService, IWordPressService wordPressService)
    {
        _workService = workService;
        _wordPressService = wordPressService;
    }

    private Catalog PageData
    {
        get
        {
            RouteData.Values.TryGetValue(nameof(Catalog), out var values);
            return values as Catalog ?? new();
        }
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid workId)
    {
        var work = await _workService.GetAsync<WordPressLister>(workId);
        if (work is null)
        {
            return View(Empty.DefaultView, new ErrorViewModel
            {
                RequestId = workId.ToString()
            });
        }
        work.Category = PageData.NormalizedName;
        work.SearchTerm = Request.Query["searchTerm"];
        var current = Request.Query["current"].ToString();
        var filterOptions = new SearchFilterOptions
        {
            SearchTerm = work.SearchTerm
        };
        if (string.IsNullOrEmpty(current))
        {
            filterOptions.Current = 1;
        }
        else
        {
            filterOptions.Current = int.Parse(current);
        }


        work.Posts = await _wordPressService.ListPostAsync(work.Domain, filterOptions) ?? new List<WordPressPost>();

        return View(work);
    }
}
