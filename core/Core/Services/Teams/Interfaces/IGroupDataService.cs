using Waffle.Models;

namespace Waffle.Core.Services.Teams.Interfaces;

public interface IGroupDataService
{
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
}
