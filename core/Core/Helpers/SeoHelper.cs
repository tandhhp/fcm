using System.Text.RegularExpressions;
using System.Text;
using Waffle.Entities;

namespace Waffle.Core.Helpers;

public class SeoHelper
{
    public static string ToSeoFriendly(string? title, int maxLength = 250, bool removeSign = true)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return string.Empty;
        }
        var match = Regex.Match(title.ToLower(), "[\\w]+");
        var result = new StringBuilder("");
        bool maxLengthHit = false;
        while (match.Success && !maxLengthHit)
        {
            if (result.Length + match.Value.Length <= maxLength)
            {
                result.Append(match.Value + "-");
            }
            else
            {
                maxLengthHit = true;
                // Handle a situation where there is only one word and it is greater than the max length.
                if (result.Length == 0) result.Append(match.Value[..maxLength]);
            }
            match = match.NextMatch();
        }
        // Remove trailing '-'
        if (result[^1] == '-') result.Remove(result.Length - 1, 1);
        if (removeSign)
        {
            return RemoveSign4VietnameseString(result.ToString());
        }
        return result.ToString();
    }

    public static string ToWikiFriendly(string? page)
    {
        if (string.IsNullOrWhiteSpace(page))
        {
            return string.Empty;
        }
        return page.Replace(" ", "_");
    }

    public static string CatalogUrl(CatalogType type)
    {
        switch (type)
        {
            case CatalogType.Default:
                return "leaf";
            case CatalogType.Article:
                return "article";
            case CatalogType.Product:
                return "product";
            case CatalogType.Location:
                return "locations";
            case CatalogType.Tag:
                return "tag";
            case CatalogType.Album:
                return "leaf/album";
            case CatalogType.Video:
                return "video";
            default: return "leaf";
        }
    }

    private static readonly string[] VietnameseSigns = new string[]
    {

        "aAeEoOuUiIdDyY",

        "áàạảãâấầậẩẫăắằặẳẵ",

        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

        "éèẹẻẽêếềệểễ",

        "ÉÈẸẺẼÊẾỀỆỂỄ",

        "óòọỏõôốồộổỗơớờợởỡ",

        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

        "úùụủũưứừựửữ",

        "ÚÙỤỦŨƯỨỪỰỬỮ",

        "íìịỉĩ",

        "ÍÌỊỈĨ",

        "đ",

        "Đ",

        "ýỳỵỷỹ",

        "ÝỲỴỶỸ"
    };

    private static string RemoveSign4VietnameseString(string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
        }
        return str;
    }

    public static string GetDomain(HttpRequest request)
    {
        if (!request.Host.HasValue || request.Host.Value.StartsWith("localhost"))
        {
            return "defzone.net";
        }
        return request.Host.Value;
    }
}
