using System.Text.RegularExpressions;

namespace Waffle.Core.Helpers;

public class PhoneNumberValidator
{
    public static bool IsValidVietnamPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
        // Trim any leading or trailing spaces
        phoneNumber = phoneNumber.Trim();

        // Define the regular expression for a valid Vietnam phone number
        // It checks for valid mobile numbers (starting with 03, 05, 07, 08, 09) and landlines (02, 03, 04...)
        string pattern = @"^(0[3-9]{1}[0-9]{8})$";

        // Match the phone number against the regular expression pattern
        var regex = new Regex(pattern);

        return regex.IsMatch(phoneNumber);
    }
}
