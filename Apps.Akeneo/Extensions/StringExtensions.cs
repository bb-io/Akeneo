namespace Apps.Akeneo.Extensions;

public static class StringExtensions
{
    public static string ToFileName(this string value, string fileType)
    {
        value = value.Replace(' ', '_').Trim();
        return $"{value}.{fileType}";
    }
}
