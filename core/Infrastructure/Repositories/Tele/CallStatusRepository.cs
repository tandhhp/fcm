using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Services.Calls.Models;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Tele;

public class CallStatusRepository(ApplicationDbContext context) : EfRepository<CallStatus>(context), ICallStatusRepository
{
    public Task<bool> IsExistAsync(string code) => _context.CallStatuses.AnyAsync(x => x.Code != null && x.Code.ToUpper() == code.ToLower());

    public async Task<bool> IsUsedAsync(int id) => await _context.CallHistories.AnyAsync(x => x.CallStatusId == id);

    public async Task<ListResult<object>> ListAsync(CallStatusFilterOptions filterOptions)
    {
        var query = from a in _context.CallStatuses
                    select new
                    {
                        a.Id,
                        a.Name,
                        CallCount = _context.CallHistories.Count(x => x.CallStatusId == a.Id)
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderByDescending(x => x.CallCount);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object> OptionsAsync(SelectOptions options) => await _context.CallStatuses.Select(x => new
    {
        Label = x.Name,
        Value = x.Id
    }).ToListAsync();
}
