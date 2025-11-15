using System.Text.Json;
using Waffle.ExternalAPI.Interfaces;
using Waffle.ExternalAPI.Models;

namespace Waffle.ExternalAPI.Services;

public class ShopeeService : IShopeeService
{
    private readonly HttpClient _http;
    private readonly ILogger<ShopeeService> _logger;
    public ShopeeService(HttpClient http, ILogger<ShopeeService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<BaseInfoAndLinks> GetBaseInfoAndLinksAsync(int pageNum)
    {
        try
        {
            var url = "https://mycollection.shop/api/v3/gql/graphql";
            var value = new BaseInfoAndLinksPayload<Variables>
            {
                OperationName = "getBaseInfoAndLinks",
                Query = "query getBaseInfoAndLinks($urlSuffix: String!, $pageSize: String, $pageNum: String, $groupId: String, $linkNameKeyword: String) {\r\n  landingPageBaseInfo(urlSuffix: $urlSuffix) {\r\n    name\r\n    headPortrait\r\n    description\r\n    region\r\n    affiliateId\r\n    shopLink\r\n    background\r\n    groupList {\r\n      groupId\r\n      groupName\r\n      groupType\r\n    }\r\n    topFiveExternalLinkImages\r\n  }\r\n  landingPageLinkList(\r\n    urlSuffix: $urlSuffix\r\n    pageSize: $pageSize\r\n    pageNum: $pageNum\r\n    groupId: $groupId\r\n    linkNameKeyword: $linkNameKeyword\r\n  ) {\r\n    totalCount\r\n    linkList {\r\n      linkId\r\n      link\r\n      linkName\r\n      image\r\n      linkType\r\n      groupIds\r\n    }\r\n  }\r\n}\r\n",
                Variables = new Variables
                {
                    PageNum = $"{pageNum}",
                    PageSize = "8",
                    UrlSuffix = "banhque",
                }
            };
            var response = await _http.PostAsJsonAsync(url, value);
            if (response.IsSuccessStatusCode)
            {
                var data = await JsonSerializer.DeserializeAsync<ShopeeResult<BaseInfoAndLinks>>(await response.Content.ReadAsStreamAsync());
                return data?.Data ?? new BaseInfoAndLinks();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Exception {Message}", ex.Message);
        }
        return new BaseInfoAndLinks();
    }

    public async Task<LandingPageLinkList> GetLinkListAsync(string? urlSuffix, string? groupId, string? searchTerm)
    {
        try
        {
            var url = "https://mycollection.shop/api/v3/gql/graphql";
            var value = new BaseInfoAndLinksPayload<Variables2>
            {
                OperationName = "getLinkLists",
                Query = "query getLinkLists($urlSuffix: String!, $pageSize: String, $pageNum: String, $groupId: String, $linkNameKeyword: String) {\r\n  landingPageLinkList(\r\n    urlSuffix: $urlSuffix\r\n    pageSize: $pageSize\r\n    pageNum: $pageNum\r\n    groupId: $groupId\r\n    linkNameKeyword: $linkNameKeyword\r\n  ) {\r\n    totalCount\r\n    linkList {\r\n      linkId\r\n      link\r\n      linkName\r\n      image\r\n      linkType\r\n      groupIds\r\n    }\r\n  }\r\n}\r\n",
                Variables = new Variables2
                {
                    PageNum = "1",
                    PageSize = "20",
                    UrlSuffix = urlSuffix,
                    GroupId = groupId,
                    LinkNameKeyWord = searchTerm
                }
            };
            var response = await _http.PostAsJsonAsync(url, value);
            if (response.IsSuccessStatusCode)
            {
                var data = await JsonSerializer.DeserializeAsync<ShopeeResult<BaseInfoAndLinks>>(await response.Content.ReadAsStreamAsync());
                return data?.Data?.LandingPageLinkList ?? new LandingPageLinkList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex.ToString());
        }
        return new LandingPageLinkList();
    }

    public async Task<LandingPageLinkList> GetLinkListsAsync(string tag, int pageNum , int pageSize)
    {
        try
        {
            var url = "https://mycollection.shop/api/v3/gql/graphql";
            var value = new BaseInfoAndLinksPayload<Variables3>
            {
                OperationName = "getLinkLists",
                Query = "query getLinkLists($urlSuffix: String!, $pageSize: String, $pageNum: String, $groupId: String, $linkNameKeyword: String) {\r\n  landingPageLinkList(\r\n    urlSuffix: $urlSuffix\r\n    pageSize: $pageSize\r\n    pageNum: $pageNum\r\n    groupId: $groupId\r\n    linkNameKeyword: $linkNameKeyword\r\n  ) {\r\n    totalCount\r\n    linkList {\r\n      linkId\r\n      link\r\n      linkName\r\n      image\r\n      linkType\r\n      groupIds\r\n    }\r\n  }\r\n}\r\n",
                Variables = new Variables3
                {
                    PageNum = pageNum.ToString(),
                    PageSize = pageSize.ToString(),
                    UrlSuffix = "banhque",
                    LinkNameKeyWord = tag
                }
            };
            var response = await _http.PostAsJsonAsync(url, value);
            if (response.IsSuccessStatusCode)
            {
                var data = await JsonSerializer.DeserializeAsync<ShopeeResult<BaseInfoAndLinks>>(await response.Content.ReadAsStreamAsync());
                return data?.Data?.LandingPageLinkList ?? new LandingPageLinkList();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(message: ex.ToString());
        }
        return new LandingPageLinkList();
    }
}
