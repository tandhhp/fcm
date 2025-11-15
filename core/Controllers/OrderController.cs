using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Waffle.Core.Constants;
using Waffle.Core.Interfaces.IService;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Entities.Ecommerces;
using Waffle.Entities.Payments;
using Waffle.Extensions;
using Waffle.ExternalAPI.Interfaces;
using Waffle.Foundations;
using Waffle.Models;
using Waffle.Models.Params.Products;

namespace Waffle.Controllers;

public class OrderController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _orderService = orderService;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync(BasicFilterOptions filterOptions) => Ok(await _orderService.ListAsync(filterOptions));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id) => Ok(await _orderService.GetDetailsAsync(id));

    [HttpGet("count")]
    public async Task<IActionResult> CountByStatusAsync([FromQuery] OrderStatus status) => Ok(await _orderService.CountByStatusAsync(status));

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var order = await _orderService.FindAsync(id);
        if (order is null) return BadRequest("Order not found!");
        await _orderService.DeleteAsync(order);
        return Ok(IdentityResult.Success);
    }

    [HttpPost("place-order"), AllowAnonymous]
    public async Task<IActionResult> AddAsync([FromBody] AddOrderRequest args)
    {
        if (!args.OrderDetails.Any()) return BadRequest("Không tìm thấy sản phẩm trong giỏ hàng!");
        var userId = User.GetClaimId();
        if (!string.IsNullOrEmpty(userId))
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return BadRequest("User not found!");
            args.Name = user.Name;
            args.PhoneNumber = user.PhoneNumber;
            args.Address = user.Address;
        }
        else
        {
            if (string.IsNullOrEmpty(args.PhoneNumber))
            {
                return BadRequest("Vui lòng cung cấp số điện thoại để đặt hàng!");
            }
            var customer = await _userManager.FindByNameAsync(args.PhoneNumber);
            if (customer is null)
            {
                customer = new ApplicationUser
                {
                    UserName = args.PhoneNumber,
                    Address = args.Address,
                    PhoneNumber = args.PhoneNumber
                };
                var result = await _userManager.CreateAsync(customer);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(customer, "Customer");
                }
            }
        }
        var order = new Order
        {
            UserId = User.GetId(),
            CreatedDate = DateTime.Now,
            Note = args.Note
        };
        await _orderService.AddAsync(order);
        await _orderService.AddOrderDetailsAsync(order.Id, args.OrderDetails);
        return Ok("/products/checkout/finish");
    }

    [HttpPost("add-transaction-feedback")]
    public async Task<IActionResult> AddTransactionFeedbackAsync([FromBody] Transaction args)
    {
        var trans = await _context.Transactions.FindAsync(args.Id);
        if (trans is null) return BadRequest("Không tìm thấy giao dịch");
        trans.Feedback = args.Feedback;
        _context.Transactions.Update(trans);

        await _context.AppLogs.AddAsync(new AppLog
        {
            CreatedDate = DateTime.Now,
            UserName = User.GetUserName(),
            Message = $"Cập nhật feedback sử dụng điểm"
        });

        await _context.SaveChangesAsync();
        return Ok();
    }
}
