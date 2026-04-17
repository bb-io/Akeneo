using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Apps.Akeneo.Helper;

public static class ContentHashHelper
{
    public static string GenerateContentHash(object? values)
    {
        if (values == null)
            return string.Empty;

        string jsonString = JsonConvert.SerializeObject(values);

        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        byte[] hashBytes = SHA256.HashData(bytes);

        return Convert.ToBase64String(hashBytes);
    }
}
