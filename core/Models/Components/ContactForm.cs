using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Contact form", Prompt = "contact-form")]
public class ContactForm : AbstractComponent
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("note")]
    public string? Note { get; set; }
    [JsonPropertyName("templateId")]
    public string? TemplateId { get; set; }
    [JsonPropertyName("chatId")]
    public string? ChatId { get; set; }
    [JsonPropertyName("finishPageId")]
    public Guid FinishPageId { get; set; }
    [JsonPropertyName("labels")]
    public ContactFormLabels Labels { get; set; } = new();
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Default";
    [JsonPropertyName("categories")]
    public List<Option> Categories { get; set; } = new();
}

public class ContactFormLabels : AbstractLabels
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("address")]
    public string? Address { get; set; }
    [JsonPropertyName("note")]
    public string? Note { get; set; }
    [JsonPropertyName("submit")]
    public string? Submit { get; set; }
    [JsonPropertyName("category")]
    public string? Category { get; set; }
    [JsonPropertyName("appointment")]
    public string? Appointment { get; set; }

}