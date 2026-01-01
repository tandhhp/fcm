using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Data;
using Waffle.Entities.Contracts;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Finances;

public class ContractLeaveVoucherUsageController(ApplicationDbContext db) : BaseController
{
    [HttpGet("list/{contractId:guid}")]
    public async Task<IActionResult> ListAsync([FromRoute] Guid contractId, [FromQuery] FilterOptions filterOptions)
    {
        if (contractId == Guid.Empty) return BadRequest("ContractId không hợp lệ!");

        var data = db.ContractLeaveVoucherUsages
            .Where(x => x.ContractId == contractId)
            .OrderByDescending(x => x.UsedDate);

        return Ok(await ListResult<object>.Success(data, filterOptions));
    }

    public record CreateArgs1(Guid ContractId, string VoucherName, DateTime UsedDate, int PeopleCount, decimal Amount);

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArgs1 args)
    {
        if (args.ContractId == Guid.Empty) return BadRequest("ContractId không hợp lệ!");
        if (string.IsNullOrWhiteSpace(args.VoucherName)) return BadRequest("Tên phiếu là bắt buộc!");
        if (args.VoucherName.Length > 256) return BadRequest("Tên phiếu tối đa 256 ký tự!");
        if (args.UsedDate == default) return BadRequest("Ngày sử dụng không hợp lệ!");
        if (args.PeopleCount <= 0) return BadRequest("Số lượng người sử dụng phải lớn hơn 0!");
        if (args.Amount < 0) return BadRequest("Số tiền không được nhỏ hơn 0!");

        var contractExists = await db.Contracts.AnyAsync(x => x.Id == args.ContractId);
        if (!contractExists) return NotFound("Không tìm thấy hợp đồng!");

        var entity = new ContractLeaveVoucherUsage
        {
            Id = Guid.NewGuid(),
            ContractId = args.ContractId,
            VoucherName = args.VoucherName.Trim(),
            UsedDate = args.UsedDate,
            PeopleCount = args.PeopleCount,
            Amount = args.Amount,
            CreatedDate = DateTime.UtcNow
        };

        db.ContractLeaveVoucherUsages.Add(entity);
        await db.SaveChangesAsync();
        return Ok(TResult.Success);
    }

    public record UpdateArgs1(Guid Id, string VoucherName, DateTime UsedDate, int PeopleCount, decimal Amount);

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateArgs1 args)
    {
        if (args.Id == Guid.Empty) return BadRequest("Id không hợp lệ!");
        if (string.IsNullOrWhiteSpace(args.VoucherName)) return BadRequest("Tên phiếu là bắt buộc!");
        if (args.VoucherName.Length > 256) return BadRequest("Tên phiếu tối đa 256 ký tự!");
        if (args.UsedDate == default) return BadRequest("Ngày sử dụng không hợp lệ!");
        if (args.PeopleCount <= 0) return BadRequest("Số lượng người sử dụng phải lớn hơn 0!");
        if (args.Amount < 0) return BadRequest("Số tiền không được nhỏ hơn 0!");

        var entity = await db.ContractLeaveVoucherUsages.FirstOrDefaultAsync(x => x.Id == args.Id);
        if (entity is null) return NotFound();

        entity.VoucherName = args.VoucherName.Trim();
        entity.UsedDate = args.UsedDate;
        entity.PeopleCount = args.PeopleCount;
        entity.Amount = args.Amount;
        entity.ModifiedDate = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(TResult.Success);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Id không hợp lệ!");

        var entity = await db.ContractLeaveVoucherUsages.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();

        db.ContractLeaveVoucherUsages.Remove(entity);
        await db.SaveChangesAsync();
        return Ok(TResult.Success);
    }
}