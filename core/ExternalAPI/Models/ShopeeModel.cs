using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Models
{
    public class ShopeeResult<T> where T : class
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

    public class BaseInfoAndLinks
    {
        [JsonPropertyName("landingPageBaseInfo")]
        public LandingPageBaseInfo LandingPageBaseInfo { get; set; } = new();
        [JsonPropertyName("landingPageLinkList")]
        public LandingPageLinkList LandingPageLinkList { get; set; } = new();
    }

    public class LandingPageBaseInfo
    {
        [JsonPropertyName("groupList")]
        public List<GroupList> GroupList { get; set; } = new();
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class GroupList
    {
        [JsonPropertyName("groupId")]
        public string? GroupId { get; set; }
        [JsonPropertyName("groupName")]
        public string? GroupName { get; set; }
        [JsonPropertyName("groupType")]
        public string? GroupType { get; set; }
    }

    public class BaseInfoAndLinksPayload<T> where T : class
    {
        [JsonPropertyName("operationName")]
        public string? OperationName { get; set; }
        [JsonPropertyName("query")]
        public string? Query { get; set; }
        [JsonPropertyName("variables")]
        public T? Variables { get; set; }
    }

    public class Variables
    {
        [JsonPropertyName("pageNum")]
        public string? PageNum { get; set; }
        [JsonPropertyName("pageSize")]
        public string? PageSize { get; set; }
        [JsonPropertyName("urlSuffix")]
        public string? UrlSuffix { get; set; }
    }

    public class Variables2
    {
        [JsonPropertyName("pageNum")]
        public string? PageNum { get; set; }
        [JsonPropertyName("pageSize")]
        public string? PageSize { get; set; }
        [JsonPropertyName("urlSuffix")]
        public string? UrlSuffix { get; set; }
        [JsonPropertyName("groupId")]
        public string? GroupId { get; set; }
        [JsonPropertyName("linkNameKeyword")]
        public string? LinkNameKeyWord { get; set; }
    }

    public class Variables3
    {
        [JsonPropertyName("pageNum")]
        public string? PageNum { get; set; }
        [JsonPropertyName("pageSize")]
        public string? PageSize { get; set; }
        [JsonPropertyName("urlSuffix")]
        public string? UrlSuffix { get; set; }
        [JsonPropertyName("linkNameKeyword")]
        public string? LinkNameKeyWord { get; set; }
    }

    public class LandingPageLinkList
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
        [JsonPropertyName("linkList")]
        public List<LinkList> LinkList { get; set; } = new();

        public string? KeyWord { get; set; }
    }

    public class LinkList
    {
        [JsonPropertyName("image")]
        public string? Image { get; set; }
        [JsonPropertyName("link")]
        public string? Link { get; set; }
        [JsonPropertyName("linkId")]
        public string? LinkId { get; set; }
        [JsonPropertyName("linkName")]
        public string? LinkName { get; set; }
        [JsonPropertyName("linkType")]
        public string? LinkType { get; set; }
    }
}
