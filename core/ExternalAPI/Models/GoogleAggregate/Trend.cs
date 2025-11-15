using System.Xml.Serialization;

namespace Waffle.ExternalAPI.Models.GoogleAggregate;

[XmlRoot(ElementName = "rss")]
public class Trend
{
    [XmlElement("channel")]
    public Channel? Channel { get; set; }
}

public class Channel
{
    [XmlElement("item")]
    public List<ChannelItem> Item { get; set; } = new();
}

public class ChannelItem
{
    [XmlElement("title")]
    public string? Title { get; set; }
    [XmlElement("description")]
    public string? Description { get; set; }
}
