using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Waffle.Core.Helpers;

public class ComponentHelper
{
    public static string GetDisplayName<T>()
    {
        if (typeof(T)
          .GetCustomAttributes(typeof(DisplayNameAttribute), true)
          .FirstOrDefault() is DisplayNameAttribute displayName)
            return displayName.DisplayName;
        return nameof(T);
    }

    public static DisplayAttribute? GetNormalizedName(string name)
    {
        var component = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t => t.Namespace == "Waffle.Models.Components" && t.IsClass && t.Name == name);
        if (component is null) return default;
        return AttributeHelper.GetDisplay(component);
    }
}
