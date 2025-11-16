using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Calendars;
using Waffle.Models.Filters;

namespace Waffle.Controllers;

public class CalendarController(ApplicationDbContext _context, IHCAService _hcaService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] CalendarFilterOptions filterOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = _context.Leads.AsNoTracking()
            .Where(x => x.BranchId == filterOptions.BranchId)
            .Where(x => x.Status != LeadStatus.Pending)
            .Where(x => x.EventDate.Month == filterOptions.Month && x.EventDate.Year == filterOptions.Year);
        if (_hcaService.IsUserInAnyRole(RoleName.Telesale, RoleName.Sales))
        {
            query = query.Where(x => x.CreatedBy == userId);
        }
        var keyIns = await query.ToListAsync();

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
        var userId = _hcaService.GetUserId();
        var query = from a in _context.Leads
                    join c in _context.Events on a.EventId equals c.Id
                    join b in _context.Users on a.CreatedBy equals b.Id into ab
                    from b in ab.DefaultIfEmpty()
                    where a.BranchId == filterOptions.BranchId && a.EventDate.Date == filterOptions.Date.Date
                    select new
                    {
                        a.Id,
                        a.Name,
                        a.Status,
                        EventName = c.Name,
                        KeyInName = b.Name,
                        KeyInId = b.Id
                    };
        if (_hcaService.IsUserInAnyRole(RoleName.Telesale, RoleName.Sales))
        {
            query = query.Where(x => x.KeyInId == userId);
        }
        return Ok(await ListResult<object>.Success(query, filterOptions));
    }
}