using Microsoft.AspNetCore.Http;
using System.Text.Json;

public static class CookieExtensions
{
    // Method to set an object in cookies
    public static void SetObject<T>(this IResponseCookies cookies, string key, T value, int? expireTime = null)
    {
        var jsonString = JsonSerializer.Serialize(value); // Use System.Text.Json.JsonSerializer for serialization
        cookies.Append(key, jsonString, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(expireTime ?? 30) });
    }

    // Method to get an object from cookies
    public static T GetObject<T>(this IRequestCookieCollection cookies, string key)
    {
        if (cookies.TryGetValue(key, out var value))
        {
            // Correct method: JsonSerializer.Deserialize from System.Text.Json
            return JsonSerializer.Deserialize<T>(value);
        }
        return default;
    }
}
