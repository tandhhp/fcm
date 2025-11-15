using System.Globalization;

namespace Waffle.Core.Helpers;

public class DateTimeHelper
{
    public static DateTime? ParseDateTime(string? input)
    {
        if (string.IsNullOrEmpty(input)) return default;
        string[] formats = [
            "dd-MM-yyyy HH:mm:ss",
            "dd/MM/yyyy HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy/MM/dd HH:mm:ss",
            "dd-MM-yyyy",
            "dd/MM/yyyy",
            "yyyy-MM-dd",
            "yyyy/MM/dd"
        ];

        if (DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result)) return result;
        return null;
    }

    public static DateOnly? ParseDateOnly(string? input)
    {
        if (string.IsNullOrEmpty(input)) return default;
        string[] formats = [
            "dd-MM-yyyy",
            "dd/MM/yyyy",
            "yyyy-MM-dd",
            "yyyy/MM/dd"
        ];

        if (DateOnly.TryParseExact(input, formats, out DateOnly result)) return result;

        return null;
    }
}
