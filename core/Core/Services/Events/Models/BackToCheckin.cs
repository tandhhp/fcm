namespace Waffle.Core.Services.Events.Models;

public class BackToCheckin
{
    public Guid KeyInId { get; set; }
    public string? Note { get; set; }
}
