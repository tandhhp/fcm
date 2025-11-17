using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Data;
using Waffle.ExternalAPI.Tel4vn.Filters;
using Waffle.ExternalAPI.Tel4vn.Results;

namespace Waffle.ExternalAPI.Tel4vn;

public class Tel4vnService(HttpClient _client, ApplicationDbContext _context, IConfiguration _configuration) : ITel4vnService
{
    public async Task<object> GetCdrAsync(CdrFilterOptions filterOptions)
    {
        var token = _configuration["Settings:Tel4vnToken"];
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var endPoint = "https://pitel04-api.tel4vn.com/v3/cdr";
        var queryParams = new Dictionary<string, string?>
        {
            { "start_date", filterOptions.StartDate.ToString("yyyy-MM-dd") + " " + "00:00:00" },
            { "end_date", filterOptions.EndDate.ToString("yyyy-MM-dd") + " " + "00:00:00" },
            { "limit", filterOptions.PageSize.ToString() },
            { "offset", (filterOptions.Current - 1).ToString() },
            { "extension", filterOptions.Extension },
            { "direction", filterOptions.Direction },
            { "status", filterOptions.Status }
        };

        var response = await _client.GetStreamAsync(QueryHelpers.AddQueryString(endPoint, queryParams));

        var query = from u in _context.Users
                    join ur in _context.UserRoles on u.Id equals ur.UserId
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where r.Name == RoleName.Telesale
                    select new
                    {
                        u.LineCode,
                        u.Name,
                        u.Gender
                    };
        var telesales = await query.ToListAsync();

        var data = await System.Text.Json.JsonSerializer.DeserializeAsync<Tel4vnCdrResult>(response);
        if (data?.Data is null) return new { };

        foreach (var item in data.Data)
        {
            var telesale = telesales.FirstOrDefault(t => t.LineCode == item.Extension);
            if (telesale != null)
            {
                item.TelesaleName = telesale.Name;
                item.TelesaleGender = telesale.Gender;
            }
        }

        return new
        {
            data.Data,
            filterOptions.PageSize,
            filterOptions.Current,
            data?.Total
        };
    }
}