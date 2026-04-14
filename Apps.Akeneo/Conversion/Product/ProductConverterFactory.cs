using System.Net.Mime;

namespace Apps.Akeneo.Conversion.Product;

public static class ProductConverterFactory
{
    public static IProductConverter GetConverter(string? fileFormat)
    {
        return fileFormat switch
        {
            null or MediaTypeNames.Text.Html => new ProductHtmlConverter(),
            MediaTypeNames.Application.Json => new ProductJsonConverter(),
            _ => throw new Exception($"Unsupported file format '{fileFormat}' was passed to ProductConverterFactory")
        };
    }
}
