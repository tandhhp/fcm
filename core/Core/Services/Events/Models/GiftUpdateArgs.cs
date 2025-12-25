using Waffle.Entities;

namespace Waffle.Core.Services.Events.Models;

public class GiftUpdateArgs : BaseEntity
{
    public string Name { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateOnly ExpiredDate { get; set; }
}
