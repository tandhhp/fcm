using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Core.Constants;
using Waffle.Core.Helpers;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Tours;
using Waffle.Extensions;
using Waffle.Foundations;
using Waffle.Models.Args;
using Waffle.Models.Filters;

namespace Waffle.Controllers;

public class FormController(ApplicationDbContext _context, ILogService _logService) : BaseController
{
    [HttpGet("list")]
    public async Task<IActionResult> ListAsync([FromQuery] TourFormFilterOptions filterOptions)
    {
        var query = from a in _context.Forms
                    join b in _context.Users on a.UserId equals b.Id
                    join c in _context.Catalogs on a.CatalogId equals c.Id into ac
                    from c in ac.DefaultIfEmpty()
                    join d in _context.Catalogs on c.ParentId equals d.Id into dc
                    from d in dc.DefaultIfEmpty()
                    select new
                    {
                        a.Id,
                        a.CatalogId,
                        a.CreatedDate,
                        a.Point,
                        a.UserId,
                        b.UserName,
                        FullName = b.Name,
                        b.Email,
                        b.PhoneNumber,
                        TourName = c.Name,
                        a.Status,
                        c.Type,
                        parentName = d.Name,
                        b.BranchId,
                        b.ContractCode
                    };

        if (filterOptions.Type != null)
        {
            query = query.Where(x => x.Type == filterOptions.Type);
        }

        if (filterOptions.Status != null)
        {
            query = query.Where(x => x.Status == filterOptions.Status);
        }
        if (!User.IsInRole(RoleName.Admin))
        {
            var user = await _context.Users.FindAsync(User.GetId());
            if (user is null) return Unauthorized();
            query = query.Where(x => x.BranchId == user.BranchId);
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.FullName))
        {
            query = query.Where(x => x.FullName.ToLower().Contains(filterOptions.FullName.ToLower()));
        }
        if (!string.IsNullOrWhiteSpace(filterOptions.ContractCode))
        {
            query = query.Where(x => x.ContractCode.ToLower().Contains(filterOptions.ContractCode.ToLower()));
        }
        query = query.OrderByDescending(x => x.CreatedDate);

        return Ok(new
        {
            data = await query.Skip((filterOptions.Current - 1) * filterOptions.PageSize).Take(filterOptions.PageSize).ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormAsync([FromRoute] Guid id)
    {
        var form = await _context.Forms.FindAsync(id);
        if (form is null)
        {
            return BadRequest("Form not found!");
        }
        var user = await _context.Users.FindAsync(form.UserId);
        return Ok(new
        {
            form,
            user,
            catalog = await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == form.CatalogId),
            tour = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == form.CatalogId),
            healthcare = await _context.Healthcares.FirstOrDefaultAsync(x => x.CatalogId == form.CatalogId)
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Form args)
    {
        try
        {
            var userId = User.GetId();
            var user = await _context.Users.FindAsync(userId);
            if (user is null) return BadRequest("Không tìm thấy người dùng!");

            var catalog = await _context.Catalogs.FindAsync(args.CatalogId);
            if (catalog is null) return BadRequest("Catalog not found!");

            var point = args.Point;
            if (catalog.Type == CatalogType.Tour)
            {
                var tour = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                if (tour is null) return BadRequest("Không tìm thấy thông tin!");
                if (tour.Point > user.Loyalty)
                {
                    return BadRequest("Bạn không đủ điểm để đăng ký!");
                }
                point = tour.Point;

                if (!await _context.Achievements.AnyAsync(x => x.NormalizedName == "first-tour" && x.UserId == user.Id))
                {
                    await _context.Achievements.AddAsync(new Achievement
                    {
                        CreatedDate = DateTime.Now,
                        Icon = "https://nuras.com.vn/achievements/2.png",
                        Name = "Đổi tour nghỉ dưỡng đầu tiên",
                        NormalizedName = "first-tour",
                        UserId = user.Id,
                        IsApproved = true,
                    });
                }
            }
            if (catalog.Type == CatalogType.Product)
            {
                var healthcare = await _context.Products.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                if (healthcare is null) return BadRequest("Không tìm thấy thông tin!");
                if (healthcare.Point > user.Loyalty)
                {
                    return BadRequest("Bạn không đủ điểm để đăng ký!");
                }
                point = healthcare.Point;
                if (!await _context.Achievements.AnyAsync(x => x.NormalizedName == "first-product" && x.UserId == user.Id))
                {
                    await _context.Achievements.AddAsync(new Achievement
                    {
                        CreatedDate = DateTime.Now,
                        Icon = "https://nuras.com.vn/achievements/3.png",
                        Name = "Đổi sản phẩm chăm sóc sức khỏe đầu tiên",
                        NormalizedName = "first-product",
                        UserId = user.Id,
                        IsApproved = true,
                    });
                }
            }
            if (catalog.Type == Entities.CatalogType.Healthcare)
            {
                var healthcare = await _context.Healthcares.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                if (healthcare is null) return BadRequest("Không tìm thấy thông tin!");
                if (healthcare.Point > user.Loyalty)
                {
                    return BadRequest("Bạn không đủ điểm để đăng ký!");
                }
                point = healthcare.Point;
                if (!await _context.Achievements.AnyAsync(x => x.NormalizedName == "first-health" && x.UserId == user.Id))
                {
                    await _context.Achievements.AddAsync(new Achievement
                    {
                        CreatedDate = DateTime.Now,
                        Icon = "https://nuras.com.vn/achievements/4.png",
                        Name = "Đặt lịch khám lần đầu",
                        NormalizedName = "first-health",
                        UserId = user.Id,
                        IsApproved = true,
                    });
                }
            }

            args.Status = FormStatus.New;
            args.Point = point;
            args.UserId = userId;
            args.CreatedDate = DateTime.Now;
            await _context.Forms.AddAsync(args);

            await _context.SaveChangesAsync();

            return Ok(IdentityResult.Success);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("histories/{userId}")]
    public async Task<IActionResult> HistoryAsync([FromRoute] Guid userId)
    {
        var query = from a in _context.Forms
                    join b in _context.Catalogs on a.CatalogId equals b.Id
                    join c in _context.Catalogs on b.ParentId equals c.Id into bc from c in bc.DefaultIfEmpty()
                    where a.UserId == userId
                    orderby a.CreatedDate descending
                    select new
                    {
                        a.Id,
                        a.CatalogId,
                        a.CreatedDate,
                        a.ScheduledDate,
                        a.Status,
                        b.Type,
                        a.Point,
                        catalogName = b.Name,
                        parentName = c.Name
                    };
        return Ok(new
        {
            data = await query.ToListAsync(),
            total = await query.CountAsync()
        });
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var form = await _context.Forms.FindAsync(id);
        if (form is null) return BadRequest("Form not found!");
        var admin = await _context.Users.FindAsync(User.GetId());
        if (admin is null) return BadRequest("Admin not found!");

        await _logService.AddAsync($"{admin.Name} đã xóa đơn đăng ký {form.Id}");

        _context.Forms.Remove(form);

        var trans = await _context.Transactions.Where(x => x.FormId == id).ToListAsync();
        if (trans.Any())
        {
            _context.Transactions.RemoveRange(trans);
        }

        await _context.SaveChangesAsync();
        return Ok(IdentityResult.Success);
    }

    [HttpPost("cx-create")]
    public async Task<IActionResult> CxCreateAsync([FromBody] CxCreateArgs args)
    {
        try
        {
            var user = await _context.Users.FindAsync(User.GetId());
            if (user is null) return Unauthorized();
            var cardHolder = await _context.Users.FindAsync(args.CardHolderId);
            if (cardHolder is null) return BadRequest("Không tìm thấy chủ thẻ!");
            var point = 0;
            if (args.Type != CatalogType.Special)
            {
                var catalog = await _context.Catalogs.FindAsync(args.CatalogId);
                if (catalog is null) return BadRequest("Không tìm thấy dịch vụ!");
                if (catalog.Type == CatalogType.Tour)
                {
                    var tour = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                    if (tour is null) return BadRequest("Tour không tìm thấy");
                    if (cardHolder.Loyalty < tour.Point) return BadRequest("Chủ thẻ không đủ điểm để sử dụng dịch vụ");
                    point = tour.Point;
                }
                if (catalog.Type == CatalogType.Product)
                {
                    var tour = await _context.Products.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                    if (tour is null) return BadRequest("Sản phẩm không tìm thấy");
                    if (cardHolder.Loyalty < tour.Point) return BadRequest("Chủ thẻ không đủ điểm để sử dụng dịch vụ");
                    point = tour.Point;
                }
                if (catalog.Type == CatalogType.Room)
                {
                    var tour = await _context.TourCatalogs.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                    if (tour is null) return BadRequest("Khách sạn không tìm thấy");
                    if (cardHolder.Loyalty < tour.Point) return BadRequest("Chủ thẻ không đủ điểm để sử dụng dịch vụ");
                    point = tour.Point;
                }
                if (catalog.Type == CatalogType.Healthcare)
                {
                    var tour = await _context.Healthcares.FirstOrDefaultAsync(x => x.CatalogId == args.CatalogId);
                    if (tour is null) return BadRequest("Gói khám không tìm thấy");
                    if (cardHolder.Loyalty < tour.Point) return BadRequest("Chủ thẻ không đủ điểm để sử dụng dịch vụ");
                    point = tour.Point;
                }
                args.Name = catalog.Name;
            }
            else
            {
                if (args.Point is null || args.Point < 1) return BadRequest("Điểm không được nhỏ hơn 0");
                point = args.Point ?? 0;
                if (string.IsNullOrEmpty(args.Name)) return BadRequest("Vui lòng nhập tên sản phẩm - dịch vụ");
                var catalog = new Catalog
                {
                    Name = args.Name,
                    Active = true,
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.Now,
                    Locale = "vi-VN",
                    ModifiedDate = DateTime.Now,
                    NormalizedName = SeoHelper.ToSeoFriendly(args.Name),
                    ViewCount = 0,
                    Type = CatalogType.Special,
                    Thumbnail = "https://nuras.com.vn/imgs/web/logo-white-full.png"
                };
                await _context.Catalogs.AddAsync(catalog);
                args.CatalogId = catalog.Id;
            }

            var form = new Form
            {
                CatalogId = args.CatalogId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Point = point,
                Status = FormStatus.InProgress,
                UserId = cardHolder.Id,
                ScheduledDate = args.ScheduledDate
            };
            await _context.Forms.AddAsync(form);
            int remainingPoints = form.Point;

            if (cardHolder.Token >= remainingPoints)
            {
                cardHolder.Token -= remainingPoints;
            }
            else
            {
                remainingPoints -= cardHolder.Token;
                cardHolder.Token = 0;
                // Nếu còn điểm cần trừ, trừ tiếp vào điểm tích lũy
                if (cardHolder.Loyalty >= remainingPoints)
                {
                    cardHolder.Loyalty -= remainingPoints;
                }
                else
                {
                    return BadRequest("Chủ thẻ không đủ điểm để sử dụng dịch vụ");
                }
            }

            await _logService.AddAsync($"Chủ thẻ: {cardHolder.Name} -> Điểm NP {cardHolder.Loyalty}, Điểm thưởng: {cardHolder.Token}, Điểm trừ: {form.Point}");

            _context.Users.Update(cardHolder);

            await _context.Transactions.AddAsync(new Entities.Payments.Transaction
            {
                CreatedDate = DateTime.Now,
                Point = -form.Point,
                UserId = cardHolder.Id,
                Memo = args.Name,
                FormId = form.Id
            });

            await _context.SaveChangesAsync();

            return Ok(IdentityResult.Success);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
