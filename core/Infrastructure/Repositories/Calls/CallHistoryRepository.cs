using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Calls;
using Waffle.Core.Services.Calls.Filters;
using Waffle.Core.Services.Calls.Models;
using Waffle.Data;
using Waffle.Entities.Contacts;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Calls;

public class CallHistoryRepository(ApplicationDbContext context) : EfRepository<CallHistory>(context), ICallHistoryRepository
{
    public async Task<ListResult<object>> GetStatusDetailsAsync(CallStatusDetailFilterOptions filterOptions)
    {
        var query = from ch in _context.CallHistories
                    join cs in _context.CallStatuses on ch.CallStatusId equals cs.Id
                    join c in _context.Contacts on ch.ContactId equals c.Id
                    select new
                    {
                        ch.Id,
                        ch.CreatedDate,
                        ch.CreatedBy,
                        CallStatus = cs.Name,
                        c.Name,
                        c.PhoneNumber,
                        c.Email,
                        ch.Note,
                        ch.Job,
                        ch.Age,
                        ch.FollowUpDate,
                        ch.TravelHabit,
                        c.UserId
                    };
        if (filterOptions.CallStatusId.HasValue)
        {
            query = query.Where(x => x.CallStatus == _context.CallStatuses.First(cs => cs.Id == filterOptions.CallStatusId.Value).Name);
        }
        if (filterOptions.TeleId.HasValue)
        {
            query = query.Where(x => x.UserId == filterOptions.TeleId.Value);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<ListResult<object>> HistoriesAsync(CallHistoryFilterOptions filterOptions)
    {
        var query = from ch in _context.CallHistories
                    join cs in _context.CallStatuses on ch.CallStatusId equals cs.Id
                    select new
                    {
                        ch.Id,
                        ch.CreatedDate,
                        ch.CreatedBy,
                        CallStatus = cs.Name,
                        ch.ContactId,
                        ch.Note,
                        ch.Job,
                        ch.Age,
                        ch.FollowUpDate,
                        ch.TravelHabit
                    };
        if (filterOptions.ContactId.HasValue)
        {
            query = query.Where(x => x.ContactId == filterOptions.ContactId);
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<TResult<object>> StatisticsAsync()
    {
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;
        var previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
        var totalCalls = await _context.CallHistories.CountAsync();
        var currentMonthCalls = await _context.CallHistories.CountAsync(ch => ch.CreatedDate.Year == currentYear && ch.CreatedDate.Month == currentMonth);
        var previousMonthCalls = await _context.CallHistories.CountAsync(ch => ch.CreatedDate.Year == currentYear && ch.CreatedDate.Month == previousMonth);
        var yearlyCalls = await _context.CallHistories.CountAsync(x => x.CreatedDate.Year == currentYear);
        var statistics = new
        {
            TotalCalls = totalCalls,
            CurrentMonthCalls = currentMonthCalls,
            PreviousMonthCalls = previousMonthCalls,
            YearlyCalls = yearlyCalls
        };
        return TResult<object>.Ok(statistics);
    }

    public async Task<object?> TeleReportAsync(TeleReportFilterOptions filterOptions)
    {
        var query = from ch in _context.CallHistories
                    join c in _context.Contacts on ch.ContactId equals c.Id
                    join cs in _context.CallStatuses on ch.CallStatusId equals cs.Id
                    join u in _context.Users on ch.CreatedBy equals u.Id
                    select new
                    {
                        ch.CreatedDate,
                        ch.CreatedBy,
                        CallStatus = cs.Name,
                        ch.CallStatusId,
                        TeleName = u.Name,
                        ManagerName = u.ManagerId != null ? _context.Users.First(m => m.Id == u.ManagerId).Name : null
                    };
        if (filterOptions.FromDate.HasValue && filterOptions.ToDate.HasValue)
        {
            query = query.Where(x => x.CreatedDate.Date >= filterOptions.FromDate.Value.Date && x.CreatedDate.Date <= filterOptions.ToDate.Value.Date);
        }
        var data = await query.Select(x => new
        {
            x.CreatedBy,
            x.TeleName,
            x.ManagerName,
            x.CallStatus,
            x.CallStatusId
        }).ToListAsync();
        var report = data
            .GroupBy(x => x.CreatedBy)
            .Select(g => new
            {
                g.First().TeleName,
                g.First().ManagerName,
                TotalCalls = g.Count(),
                CallStatusCounts = g.GroupBy(x => new { x.CallStatus, x.CallStatusId })
                                    .Select(cs => new
                                    {
                                        cs.Key.CallStatus,
                                        Count = cs.Count(),
                                        TeleId = g.Key,
                                        cs.Key.CallStatusId
                                    }).ToList()
            }).ToList();
        return TResult<object>.Ok(report);
    }
}
