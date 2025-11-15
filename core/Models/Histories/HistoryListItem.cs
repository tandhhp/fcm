using Waffle.Entities;

namespace Waffle.Models.Histories;

public class HistoryListItem : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public string Message { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
