using Waffle.Core.Interfaces.IRepository.Settings;
using Waffle.Core.Services.Cards.Args;
using Waffle.Core.Services.Cards.Filters;
using Waffle.Entities;
using Waffle.Models;

namespace Waffle.Core.Services.Cards;

public class CardService(ICardRepository _cardRepository) : ICardService
{
    public async Task<TResult> CreateAsync(CardCreateArgs args)
    {
        await _cardRepository.AddAsync(new Card
        {
            Name = args.Name,
            Code = args.Code
        });
        return TResult.Success;
    }

    public async Task<TResult> DeleteAsync(Guid id)
    {
        var data = await _cardRepository.FindAsync(id);
        if (data is null) return TResult.Failed("Không tìm thấy thẻ!");
        await _cardRepository.DeleteAsync(data);
        return TResult.Success;
    }

    public async Task<TResult<object>> DetailAsync(Guid id)
    {
        var data = await _cardRepository.FindAsync(id);
        if (data is null) return TResult<object>.Failed("Không tìm thấy thẻ!");
        return TResult<object>.Ok(new
        {
            data.Id,
            data.Name,
            data.Code
        });
    }

    public Task<ListResult<object>> ListAsync(CardFilterOptions filterOptions) => _cardRepository.ListAsync(filterOptions);

    public Task<object> OptionsAsync(SelectOptions selectOptions) => _cardRepository.GetOptionsAsync(selectOptions);

    public async Task<TResult> UpdateAsync(CardUpdateArgs args)
    {
        var data = await _cardRepository.FindAsync(args.Id);
        if (data is null) return TResult.Failed("Không tìm thấy thẻ!");
        data.Code = args.Code;
        data.Name = args.Name;
        await _cardRepository.UpdateAsync(data);
        return TResult.Success;
    }
}
