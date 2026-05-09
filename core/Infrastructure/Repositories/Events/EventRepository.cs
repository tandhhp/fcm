using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Events;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Services.Events.Filters;
using Waffle.Core.Services.Events.Models;
using Waffle.Core.Services.Events.Results;
using Waffle.Core.Services.Leads.Filters;
using Waffle.Core.Services.Tables.Filters;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Contracts;
using Waffle.Models;
using Waffle.Models.Components;

namespace Waffle.Infrastructure.Repositories.Events;

public class EventRepository(ApplicationDbContext context, IHCAService _hcaService) : EfRepository<Event>(context), IEventRepository
{
    public async Task<TResult> CreateContractAsync(Lead lead, string contractCode, decimal amount, Guid? cardId, int sourceId)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == lead.CreatedBy);
        if (user is null) return TResult.Failed("Người tạo không hợp lệ!");
        if (lead.SalesId is null) return TResult.Failed("Nhân viên kinh doanh không hợp lệ!");
        if (!await _context.Sources.AnyAsync(x => x.Id == sourceId)) return TResult.Failed("Nguồn không hợp lệ!");
        await _context.Contracts.AddAsync(new Contract
        {
            Amount = amount,
            CreatedBy = _hcaService.GetUserId(),
            CardId = cardId,
            Code = contractCode,
            CreatedDate = DateTime.Now,
            SalesId = lead.SalesId,
            ToById = lead.ToById,
            SourceId = sourceId,
            KeyInId = lead.CreatedBy,
            TeamKeyInId = user.ManagerId,
            LeadId = lead.Id
        });
        return TResult.Success;
    }

    public async Task<ListResult<object>> GetListAsync(EventFilterOptions filterOptions)
    {
        var query = from e in _context.Events
                    select new
                    {
                        e.Id,
                        e.Name,
                        e.CreatedDate
                    };
        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(filterOptions.Name.ToLower()));
        }
        query = query.OrderByDescending(x => x.CreatedDate);
        return await ListResult<object>.Success(query, filterOptions);
    }

    public async Task<object> KeyInOptionsAsync(KeyInSelectOptions selectOptions)
    {
        var userId = _hcaService.GetUserId();
        var query = from u in _context.Users
                    join ur in _context.UserRoles on u.Id equals ur.UserId
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where r.Name == RoleName.Sales || r.Name == RoleName.Telesales
                    where u.Status == UserStatus.Working
                    select new
                    {
                        u.Id,
                        u.Name,
                        u.ManagerId
                    };
        if (selectOptions.ManagerId.HasValue)
        {
            query = query.Where(x => x.ManagerId == selectOptions.ManagerId);
        }
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new {
            Value = x.Id,
            Label = x.Name
        }).ToListAsync();
    }

    public async Task<object> OptionsAsync() => await _context.Events
        .OrderByDescending(x => x.Name)
        .Select(x => new
        {
            Value = x.Id,
            Label = x.Name
        }).ToListAsync();

    public async Task<List<SUReportResult>> SuReportAsync(SUFilterOptions filterOptions)
    {
        var currentUserId = _hcaService.GetUserId();
        var salesManagersQuery = from u in _context.Users
                            join ur in _context.UserRoles on u.Id equals ur.UserId
                            join r in _context.Roles on ur.RoleId equals r.Id
                            where r.Name == RoleName.SalesManager && u.Status == UserStatus.Working
                            select new
                            {
                                u.Id,
                                u.Name,
                                u.DotId,
                                u.ManagerId
                            };
        if (filterOptions.SalesManagerId.HasValue)
        {
            salesManagersQuery = salesManagersQuery.Where(x => x.Id == filterOptions.SalesManagerId);
        }
        if (filterOptions.DosId.HasValue)
        {
            salesManagersQuery = salesManagersQuery.Where(x => x.ManagerId == filterOptions.DosId);
        }
        if (_hcaService.IsUserInRole(RoleName.Sales))
        {
            var currentUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == currentUserId);
            if (currentUser is null) return [];
            salesManagersQuery = salesManagersQuery.Where(x => x.ManagerId == currentUser.ManagerId);
        }
        var salesManagers = await salesManagersQuery.AsNoTracking().ToListAsync();
        var salesQuery = from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.Id
                         where r.Name == RoleName.Sales && u.Status == UserStatus.Working
                         select new
                         {
                             u.Id,
                             u.Name,
                             u.SmId
                         };
        var sales = await salesQuery.AsNoTracking().ToListAsync();

        var attendances = await _context.Attendances.AsNoTracking().ToListAsync();

        var leadQuery = from l in _context.Leads
                   where l.EventDate.Date >= filterOptions.FromDate.Date && l.EventDate.Date <= filterOptions.ToDate.Date
                   select new
                   {
                       l.Id,
                       l.CreatedBy,
                       l.AttendanceId
                   };
        var leads = await leadQuery.ToListAsync();

        var result = new List<SUReportResult>();
        foreach (var sm in salesManagers)
        {
            var smSales = sales.Where(x => x.SmId == sm.Id).ToList();
            var suReport = new SUReportResult
            {
                SalesManagerName = sm.Name
            };
            var salesReports = new List<SUSalesReport>();
            var salesBySm = sales.Where(x => x.SmId == sm.Id).ToList();
            foreach (var sale in salesBySm)
            {
                var suSalesReport = new SUSalesReport
                {
                    SalesName = sale.Name
                };
                var suAttendances = new List<SUAttendance>();
                var totalCountRate = 0f;
                var totalKeyInCount = 0;
                foreach (var attendance in attendances)
                {
                    var count = leads.Count(x => x.CreatedBy == sale.Id && x.AttendanceId == attendance.Id);
                    suAttendances.Add(new SUAttendance
                    {
                        AttendanceId = attendance.Id,
                        Count = count,
                        Name = attendance.Name
                    });
                    totalKeyInCount += count;
                    totalCountRate += count * attendance.SuRate;
                }
                suSalesReport.Attendances = suAttendances;
                suSalesReport.TotalKeyInCount = totalKeyInCount;
                suSalesReport.TotalRate = totalCountRate;
                salesReports.Add(suSalesReport);
            }
            suReport.SalesReports = salesReports;
            result.Add(suReport);
        }
        return result;
    }

    public async Task<object?> TableOptionsAsync(AllTableFilterOptions filterOptions)
    {
        var rooms = await _context.Rooms.Where(x => x.BranchId == filterOptions.BranchId).AsNoTracking().ToListAsync();
        var tables = await (from t in _context.Tables
                            join r in _context.Rooms on t.RoomId equals r.Id
                            where filterOptions.BranchId == null || r.BranchId == filterOptions.BranchId
                            select t).ToListAsync();

        var eventTables = await (from l in _context.Leads
                                 join f in _context.LeadFeedbacks on l.Id equals f.LeadId
                                 where l.EventId == filterOptions.EventId && l.EventDate.Date == filterOptions.EventDate.Date
                                 select f.TableId
                                 ).ToListAsync();

        var result = new List<OptionGroup>();
        foreach (var room in rooms)
        {
            var optionGroup = new OptionGroup
            {
                Label = room.Name,
                Options = []
            };
            var roomTables = tables.Where(x => x.RoomId == room.Id);
            var options = new List<Option>();
            foreach (var table in roomTables)
            {
                options.Add(new Option
                {
                    Value = table.Id,
                    Label = table.Name,
                    Disabled = eventTables.Any(x => x == table.Id)
                });
            }
            optionGroup.Options = options;
            result.Add(optionGroup);
        }
        return result;
    }

    public async Task<object> ToOptionsAsync(SelectOptions selectOptions)
    {
        var query = from u in _context.Users
                    join ur in _context.UserRoles on u.Id equals ur.UserId
                    join r in _context.Roles on ur.RoleId equals r.Id
                    where r.Name == RoleName.SalesManager
                    where u.Status == UserStatus.Working
                    select new
                    {
                        u.Id,
                        u.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new {
            Value = x.Id,
            Label = x.Name
        }).ToListAsync();
    }
}
