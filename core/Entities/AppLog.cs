using Microsoft.IdentityModel.Abstractions;

namespace Waffle.Entities;

public class AppLog : BaseEntity
{
    public string Message { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public EventLogLevel Level { get; set; }
    public string UserName { get; set; } = default!;
}
