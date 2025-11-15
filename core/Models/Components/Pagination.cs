namespace Waffle.Models.Components;

public class Pagination : FilterOptions, IFilterOptions
{
    public string? Token { get; set; }
    public bool HasPrevPage => Type == PaginationType.Default ? Current > 1 : string.IsNullOrEmpty(PrevToken);
    public bool HasNextPage => Type == PaginationType.Default ? Total > Current * PageSize : string.IsNullOrEmpty(PrevToken);
    public string? NextPageUrl { get; set; }
    public string? PrevPageUrl { get; set; }
    public int Total { get; set; }
    public PaginationType Type { get; set; }
    public string? PrevToken { get; set; }
    public string? NextToken { get; set; }
}

public enum PaginationType
{
    Default,
    Token
}
