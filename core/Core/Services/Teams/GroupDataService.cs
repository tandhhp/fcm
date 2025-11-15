using Waffle.Core.Interfaces.IRepository;
using Waffle.Core.Services.Teams.Interfaces;
using Waffle.Models;

namespace Waffle.Core.Services.Teams;

public class GroupDataService(IGroupDataRepository _groupDataRepository) : IGroupDataService
{
    public Task<object> GetOptionsAsync(SelectOptions selectOptions) => _groupDataRepository.GetOptionsAsync(selectOptions);
}
