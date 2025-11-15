using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Models;

namespace Waffle.Core.Services.Teams;

public class CallCenterService(ICallCenterRepository _callCenterRepository) : ICallCenterService
{
    public Task<object> GetOptionsAsync(SelectOptions selectOptions) => _callCenterRepository.GetOptionsAsync(selectOptions);
}
