using Waffle.Entities;

namespace Waffle.Models.Args;

public class TopupArgs
{
    public Guid CardHolderId { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public TopupType Type { get; set; }
}
