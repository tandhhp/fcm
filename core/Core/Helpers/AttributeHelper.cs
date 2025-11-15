using System.ComponentModel.DataAnnotations;

namespace Waffle.Core.Helpers;

public class AttributeHelper
{
    private static T? RetrieveCustomAttribute<T>(Type type) where T : Attribute
    {
        return Attribute.GetCustomAttribute(type, typeof(T)) as T;
    }

    public static DisplayAttribute? GetDisplay(Type type) => RetrieveCustomAttribute<DisplayAttribute>(type);
}
