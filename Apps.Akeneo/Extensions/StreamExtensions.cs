namespace Apps.Akeneo.Extensions;

public static class StreamExtensions
{
    public static async Task<string> ReadAsStringAsync(this Stream stream)
    {
        using var memoryStream = new MemoryStream();

        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync();
    }
}
