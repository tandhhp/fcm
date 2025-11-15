using System.Xml.Serialization;

namespace Waffle.Core.Helpers;

public class XmlHelper
{
    public static T? Deserialize<T>(Stream input) where T : class
    {
        var ser = new XmlSerializer(typeof(T));
        return ser.Deserialize(input) as T;
    }

    public string Serialize<T>(T objectToSerialize)
    {
        if (objectToSerialize is null)
        {
            return string.Empty;
        }
        var xmlSerializer = new XmlSerializer(objectToSerialize.GetType());

        using var textWriter = new StringWriter();
        xmlSerializer.Serialize(textWriter, objectToSerialize);
        return textWriter.ToString();
    }
}
