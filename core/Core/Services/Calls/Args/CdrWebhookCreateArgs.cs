using System.Text.Json.Serialization;

namespace Waffle.Core.Services.Calls.Args;

public class CdrWebhookCreateArgs
{
    [JsonPropertyName("application")]
    public string? Application { get; set; }

    [JsonPropertyName("billsec")]
    public int? Billsec { get; set; }

    [JsonPropertyName("call_id")]
    public string? CallId { get; set; }

    [JsonPropertyName("campaign_uuid")]
    public string? CampaignUuid { get; set; }

    [JsonPropertyName("direction")]
    public string? Direction { get; set; }

    [JsonPropertyName("domain")]
    public string? Domain { get; set; }

    [JsonPropertyName("domain_uuid")]
    public string? DomainUuid { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("from_number")]
    public string? FromNumber { get; set; }

    [JsonPropertyName("hotline")]
    public string? Hotline { get; set; }

    [JsonPropertyName("lead_uuid")]
    public string? LeadUuid { get; set; }

    [JsonPropertyName("press_key")]
    public string? PressKey { get; set; }

    [JsonPropertyName("receive_dest")]
    public string? ReceiveDest { get; set; }

    [JsonPropertyName("recording_url")]
    public string? RecordingUrl { get; set; }

    [JsonPropertyName("ref_id")]
    public string? RefId { get; set; }

    [JsonPropertyName("sip_call_id")]
    public string? SipCallId { get; set; }

    [JsonPropertyName("sip_hangup_disposition")]
    public string? SipHangupDisposition { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("time_answered")]
    public string? TimeAnswered { get; set; }

    [JsonPropertyName("time_ended")]
    public string? TimeEnded { get; set; }

    [JsonPropertyName("time_started")]
    public string? TimeStarted { get; set; }

    [JsonPropertyName("to_number")]
    public string? ToNumber { get; set; }
}