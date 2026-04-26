using System.ComponentModel.DataAnnotations;

namespace Waffle.Entities.Contacts;

public class CallWebhookLog : BaseEntity
{
    [StringLength(128)]
    public string? Application { get; set; }
    public int? Billsec { get; set; }
    [StringLength(128)]
    public string? CallId { get; set; }
    [StringLength(128)]
    public string? CampaignUuid { get; set; }
    [StringLength(64)]
    public string? Direction { get; set; }
    [StringLength(256)]
    public string? Domain { get; set; }
    [StringLength(128)]
    public string? DomainUuid { get; set; }
    public int? Duration { get; set; }
    [StringLength(64)]
    public string? FromNumber { get; set; }
    [StringLength(64)]
    public string? Hotline { get; set; }
    [StringLength(128)]
    public string? LeadUuid { get; set; }
    [StringLength(64)]
    public string? PressKey { get; set; }
    [StringLength(128)]
    public string? ReceiveDest { get; set; }
    [StringLength(2048)]
    public string? RecordingUrl { get; set; }
    [StringLength(128)]
    public string? RefId { get; set; }
    [StringLength(512)]
    public string? SipCallId { get; set; }
    [StringLength(128)]
    public string? SipHangupDisposition { get; set; }
    [StringLength(64)]
    public string? State { get; set; }
    [StringLength(64)]
    public string? Status { get; set; }
    public DateTime? TimeAnswered { get; set; }
    public DateTime? TimeEnded { get; set; }
    public DateTime? TimeStarted { get; set; }
    [StringLength(64)]
    public string? ToNumber { get; set; }
    public DateTime ReceivedDate { get; set; }
}