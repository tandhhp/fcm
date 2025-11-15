using Waffle.ExternalAPI.Tel4vn.Filters;

namespace Waffle.ExternalAPI.Tel4vn;

public interface ITel4vnService
{
    Task<object> GetCdrAsync(CdrFilterOptions filterOptions);
}
