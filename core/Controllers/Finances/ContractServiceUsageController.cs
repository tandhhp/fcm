using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Waffle.Data;
using Waffle.Entities.Contracts;
using Waffle.Foundations;
using Waffle.Models;

namespace Waffle.Controllers.Finances;

public class ContractServiceUsageController(ApplicationDbContext db) : BaseController
{
    [HttpGet("list/{contractId:guid}")]
    public async Task<IActionResult> ListAsync([FromRoute] Guid contractId, [FromQuery] FilterOptions filterOptions)
    {
        var data =  db.ContractServiceUsages
            .Where(x => x.ContractId == contractId)
            .OrderByDescending(x => x.UsedDate);
        return Ok(await ListResult<object>.Success(data, filterOptions));
    }

    public record CreateArgs(Guid ContractId, string ServiceName, DateTime UsedDate, int PeopleCount, decimal Amount);

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateArgs args)
    {
        var entity = new ContractServiceUsage
        {
            Id = Guid.NewGuid(),
            ContractId = args.ContractId,
            ServiceName = args.ServiceName,
            UsedDate = args.UsedDate,
            PeopleCount = args.PeopleCount,
            Amount = args.Amount,
            CreatedDate = DateTime.UtcNow
        };
        db.ContractServiceUsages.Add(entity);
        await db.SaveChangesAsync();
        return Ok(entity);
    }

    public record UpdateArgs(Guid Id, string ServiceName, DateTime UsedDate, int PeopleCount, decimal Amount);

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateArgs args)
    {
        var entity = await db.ContractServiceUsages.FirstOrDefaultAsync(x => x.Id == args.Id);
        if (entity is null) return NotFound();

        entity.ServiceName = args.ServiceName;
        entity.UsedDate = args.UsedDate;
        entity.PeopleCount = args.PeopleCount;
        entity.Amount = args.Amount;
        entity.ModifiedDate = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var entity = await db.ContractServiceUsages.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null) return NotFound();
        db.ContractServiceUsages.Remove(entity);
        await db.SaveChangesAsync();
        return Ok();
    }
}