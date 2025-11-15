using Waffle.Models;

namespace Waffle.Core.Services.Teams.Interfaces;

public interface ICallCenterService
{
    Task<object> GetOptionsAsync(SelectOptions selectOptions);
}
