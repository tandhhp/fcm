using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Waffle.Core.Foundations;

namespace Waffle.Models.Components;

[Display(Name = "Exchange Rate", Prompt = "exchange-rate")]
public class ExchangeRate : AbstractComponent
{
    [JsonIgnore]
    public ExrateList ExrateList { get; set; } = new();
    [JsonPropertyName("labels")]
    public ExchangeRateLabels Labels { get; set; } = new();
}

public class ExchangeRateLabels : AbstractLabels
{
    public string? CurrencyName { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Buy { get; set; }
    public string? Transfer { get; set; }
    public string? Sell { get; set; }
}


[XmlRoot(ElementName = "ExrateList")]
public class ExrateList
{
    [XmlElement(ElementName = "DateTime")]
    public string? DateTime { get; set; }
    [XmlElement(ElementName = "Exrate")]
    public List<Exrate> Exrate { get; set; } = new();
}

public class Exrate
{
    [XmlAttribute(AttributeName = "CurrencyCode")]
    public string? CurrencyCode { get; set; }

    [XmlAttribute(AttributeName = "CurrencyName")]
    public string? CurrencyName { get; set; }

    [XmlAttribute(AttributeName = "Buy")]
    public string? Buy { get; set; }

    [XmlAttribute(AttributeName = "Transfer")]
    public string? Transfer { get; set; }

    [XmlAttribute(AttributeName = "Sell")]
    public string? Sell { get; set; }
}
