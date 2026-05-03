using Waffle.Models;

namespace Waffle.Core.Services.Calls.Models;

public class CallWebhookLogFilterOptions : FilterOptions
{
    public int? BillsecFrom { get; set; }
    public int? BillsecTo { get; set; }
    public string? Direction { get; set; }
    public int? DurationFrom { get; set; }
    public int? DurationTo { get; set; }
    public string? FromNumber { get; set; }
    public string? Hotline { get; set; }
    public string? LeadUuid { get; set; }
    public string? PressKey { get; set; }
    public string? ReceiveDest { get; set; }
    public string? RecordingUrl { get; set; }
    public string? SipHangupDisposition { get; set; }
    public string? Status { get; set; }
    public DateTime? TimeAnsweredFrom { get; set; }
    public DateTime? TimeAnsweredTo { get; set; }
    public DateTime? TimeEndedFrom { get; set; }
    public DateTime? TimeEndedTo { get; set; }
    public DateTime? TimeStartedFrom { get; set; }
    public DateTime? TimeStartedTo { get; set; }
    public string? ToNumber { get; set; }
    public DateTime? ReceivedDateFrom { get; set; }
    public DateTime? ReceivedDateTo { get; set; }
}
