using Waffle.Entities;

namespace Waffle.Core.Services.Events.Models;

public class GiftUpdateArgs : BaseEntity
{
    public string Name { get; set; } = default!;
}
