using Google.Apis.Auth;

namespace Waffle.ExternalAPI.Googles;

public class GoogleApiHelper
{
    public static async Task<GoogleJsonWebSignature.Payload?> GetUserCredential(string? credential)
    {
        if (string.IsNullOrEmpty(credential)) return default;
        var settings = new GoogleJsonWebSignature.ValidationSettings();

        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

        return payload;
    }
}
