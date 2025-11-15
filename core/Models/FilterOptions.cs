using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;
using Waffle.Core.Constants;
using Waffle.Entities;

namespace Waffle.Models;

public interface IFilterOptions
{
    int Current { get; set; }
    int PageSize { get; set; }
    int? BranchId { get; set; }
}

public class FilterOptions : IFilterOptions
{
    [JsonPropertyName("current")]
    public int Current { get; set; } = 1;
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;
    public int? BranchId { get; set; }
}

public class FileFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public string? Type { get; set; }
}

public class CatalogFilterOptions : FilterOptions
{
    #region Search
    public string? Name { get; set; }
    public bool? Active { get; set; }
    public CatalogType? Type { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? CreatedBy { get; set; }
    public string? Locale { get; set; }
    #endregion

    #region Sort order
    public SortOrder? ViewCount { get; set; }
    public SortOrder? ModifiedDate { get; set; }
    #endregion
}

public class ProductFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public bool? Active { get; set; }
    public Guid? ParentId { get; set; }
    public string? Locale { get; set; }
}

public class SearchFilterOptions : FilterOptions
{
    public string? SearchTerm { get; set; }
}

public class WorkFilterOptions : FilterOptions
{
    public string? Name { get; set; }
    public Guid? ParentId { get; set; }
    public bool? Active { get; set; }
}

public class BasicFilterOptions : FilterOptions
{

}

public class ComponentFilterOptions : FilterOptions
{
    public string? Name { get; set; }
}

public class ArticleFilterOptions : FilterOptions
{
    public string? Name { get; set; }
}

public class ArticleRelatedFilterOption : FilterOptions
{
    public IEnumerable<Guid> TagIds { get; set; } = null!;
    public Guid CatalogId { get; set; }
    public CatalogType Type { get; set; }
}

public class TagFilterOptions : FilterOptions
{
    public string? KeyWords { get; set; }
    public OrderBy? OrderBy { get; set; }
}

public class LocalizationFilterOptions: FilterOptions
{
    public string? Key { get; set; }
    public string Locale { get; set; } = "vi-VN";
}

public class SelectFilterOptions
{
    public string? KeyWords { get; set; }
    public CatalogType? Type { get; set; }
    public string Locale { get; set; } = "vi-VN";
    public Guid? ParentId { get; set; }
}

public class UserFilterOptions: BasicFilterOptions
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? SmId { get; set; }
    public Guid? SalesId { get; set; }
    public Guid? CardId { get; set; }
    public string? ContractCode { get; set; }
    public DateTime? EventDate { get; set; }
    public bool IsAdmin { get; set; }
    public Guid? TmId { get; set; }
    public UserStatus? Status { get; set; }
    public Guid? DosId { get; set; }
}