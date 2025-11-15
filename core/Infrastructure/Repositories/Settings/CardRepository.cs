using Microsoft.EntityFrameworkCore;
using Waffle.Core.Foundations;
using Waffle.Core.Interfaces.IRepository.Settings;
using Waffle.Core.Services.Cards.Filters;
using Waffle.Data;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Infrastructure.Repositories.Settings;

public class CardRepository(ApplicationDbContext context) : EfRepository<Card>(context), ICardRepository
{
    public async Task<object> GetOptionsAsync(SelectOptions selectOptions)
    {
        var query = from c in _context.Cards
                    select new
                    {
                        c.Id,
                        c.Name
                    };
        if (!string.IsNullOrWhiteSpace(selectOptions.KeyWords))
        {
            query = query.Where(x => x.Name.ToLower().Contains(selectOptions.KeyWords.ToLower()));
        }
        return await query.Select(x => new
        {
            Label = x.Name,
            Value = x.Id
        }).ToListAsync();
    }

    public async Task<ListResult<object>> ListAsync(CardFilterOptions filterOptions)
    {
        var query = from c in _context.Cards
                    select new
                    {
                        c.Id,
                        c.Name,
                        c.Code,
                        ContractCount = _context.Contracts.Count(x => x.CardId == c.Id)
                    };
        query = query.OrderByDescending(x => x.Id);
        return await ListResult<object>.Success(query, filterOptions);
    }
}
