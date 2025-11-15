using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Waffle.Entities.Contacts;
using Waffle.Entities.Users;

namespace Waffle.Entities;

public class LeadFeedback : BaseEntity
{
    [ForeignKey(nameof(Lead))]
    public Guid LeadId { get; set; }
    /// <summary>
    /// Tình hình tài chính
    /// </summary>
    [Comment("Tình hình tài chính")]
    public string? FinancialSituation { get; set; }
    [Comment("Mức độ quan tâm")]
    public int InterestLevel { get; set; }
    public string? RejectReason { get; set; }
    public TimeSpan? CheckinTime { get; set; }
    public TimeSpan? CheckoutTime { get; set; }
    [ForeignKey(nameof(JobKind))]
    public int? JobKindId { get; set; }
    [ForeignKey(nameof(Table))]
    public int? TableId { get; set; }
    [ForeignKey(nameof(Transport))]
    public int? TransportId { get; set; }

    public virtual Source? Source { get; set; }
    public virtual Table? Table { get; set; }
    public virtual JobKind? JobKind { get; set; }
    public virtual Transport? Transport { get; set; }
    public virtual Lead? Lead { get; set; }
}
