using System.Text.Json.Serialization;

namespace Waffle.ExternalAPI.Models;

public class Telegram
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;
    [JsonPropertyName("chatId")]
    public string ChatId { get; set; } = string.Empty;
}

public class TelegramMessage
{
    public string Message { get; set; } = string.Empty;
}

public class TelegramResponse
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }
}