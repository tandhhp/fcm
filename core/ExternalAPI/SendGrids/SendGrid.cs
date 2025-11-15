using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.SendGrids;

public class SendGrid
{
    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;
    [JsonPropertyName("from")]
    public SendGridConfigureFrom From { get; set; } = new();
}

public class SendGridConfigureFrom
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = "f7deat@gmail.com";
    [JsonPropertyName("name")]
    public string Name { get; set; } = "Taan";
}
