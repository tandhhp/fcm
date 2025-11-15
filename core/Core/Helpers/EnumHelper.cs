using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Waffle.Core.Helpers;

public class EnumHelper
{
    public static string GetDisplayName(Enum enumValue)
    {
        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.GetName() ?? enumValue.ToString();
    }
}
