using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.GoogleAggregate;

public class Google
{
    [JsonPropertyName("bloggerApiKey")]
    public string BloggerApiKey { get; set; } = default!;
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; } = default!;
    [JsonPropertyName("firebase")]
    public Firebase Firebase { get; set; } = new();
    [JsonPropertyName("gTagId")]
    public string GTagId { get; set; } = default!;
    [JsonPropertyName("bloggers")]
    public List<Blogger> Bloggers { get; set; } = new();
}

public class Blogger
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class Firebase
{
    [JsonPropertyName("apiKey")]
    public string? ApiKey { get; set; }
    [JsonPropertyName("authDomain")]
    public string? AuthDomain { get; set; }
    [JsonPropertyName("databaseURL")]
    public string? DatabaseURL { get; set; }
    [JsonPropertyName("projectId")]
    public string? ProjectId { get; set; }
    [JsonPropertyName("storageBucket")]
    public string? StorageBucket { get; set; }
    [JsonPropertyName("messagingSenderId")]
    public string? MessagingSenderId { get; set; }
    [JsonPropertyName("appId")]
    public string? AppId { get; set; }
    [JsonPropertyName("measurementId")]
    public string? MeasurementId { get; set; }
}