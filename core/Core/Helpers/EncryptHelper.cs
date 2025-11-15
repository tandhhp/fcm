using System.Security.Cryptography;
using System.Text;

namespace Waffle.Core.Helpers;

public class EncryptHelper
{
    public static string MD5Create(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        using MD5 hash = MD5.Create();
        return string.Join("", from ba in hash.ComputeHash(Encoding.UTF8.GetBytes(input)) select ba.ToString("x2"));
    }
}
