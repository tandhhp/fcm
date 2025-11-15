using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Calendars;
using Waffle.Models.Filters;

namespace Waffle.Controllers;

public class CalendarController(ApplicationDbContext _context) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] CalendarFilterOptions filterOptions)
    {
        var keyIns = await _context.Leads.AsNoTracking()
            .Where(x => x.BranchId == filterOptions.BranchId)
            .Where(x => x.Status != LeadStatus.Pending)
            .Where(x => x.EventDate.Month == filterOptions.Month && x.EventDate.Year == filterOptions.Year).ToListAsync();

        var calendarData = new List<CalendarListData>();
        foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(filterOptions.Year, filterOptions.Month)))
        {
            var calendarListData = new CalendarListData { Day = day };
            var items = keyIns.Where(x => x.EventDate.Day == day).ToList();
            foreach (var item in items)
            {
                calendarListData.Items.Add(new CalendarListItem { Content = item.Name });
            }
            calendarListData.EventCount = items.Count;

            calendarData.Add(calendarListData);
        }
        return Ok(new { data = calendarData });
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEventsAsync([FromQuery] CalendarEventFilterOptions filterOptions)
    {
        var query = from a in _context.Leads
                    join c in _context.Events on a.EventId equals c.Id
                    join b in _context.Users on a.SalesId equals b.Id into sales
                    from b in sales.DefaultIfEmpty()
                    where a.BranchId == filterOptions.BranchId && a.EventDate.Date == filterOptions.Date.Date
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.Status,
                        EventName = c.Name,
                        Seller = b.Name
                    };
        return Ok(await ListResult<object>.Success(query, filterOptions));
    }
}