using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Tel4vn.Results;

public class Tel4vnCdrResult
{
    [JsonPropertyName("limit")]
    public int Limit { get; set; }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("data")]
    public List<Tel4vnCdrData>? Data { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class Tel4vnCdrData
{
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("extension")]
    public string? Extension { get; set; }
    [JsonPropertyName("callee_id_name")]
    public string? CalleeIdName { get; set; }
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    [JsonPropertyName("network")]
    public string? Network { get; set; }
    [JsonPropertyName("time_started")]
    public DateTime TimeStarted { get; set; }
    [JsonPropertyName("time_ended")]
    public DateTime TimeEnded { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("direction")]
    public string? Direction { get; set; }
    [JsonPropertyName("recording_url")]
    public string? RecordingUrl { get; set; }
    public string? TelesaleName { get; set; }
    public bool? TelesaleGender { get; set; }
}
