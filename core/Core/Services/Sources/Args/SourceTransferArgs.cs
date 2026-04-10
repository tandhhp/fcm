namespace Waffle.Core.Services.Sources.Args;

public class SourceTransferArgs
{
    /// <summary>
    /// Source ID hiện tại (nguồn cũ)
    /// </summary>
    public int FromSourceId { get; set; }
    
    /// <summary>
    /// Source ID đích (nguồn mới)
    /// </summary>
    public int ToSourceId { get; set; }
    
    /// <summary>
    /// Danh sách Contact IDs cần transfer. Nếu null hoặc rỗng sẽ transfer toàn bộ contacts của source
    /// </summary>
    public List<Guid>? ContactIds { get; set; }
    
    /// <summary>
    /// Có transfer cả contacts đã được assign cho telesales không
    /// </summary>
    public bool IncludeAssigned { get; set; }
}
