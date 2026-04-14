using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Akeneo.Conversion.Product;

public class ProductConverterFactory(IFileManagementClient fileManagementClient)
{
    public IProductConverter Create(string? fileFormat)
    {
        return fileFormat switch
        {
            null or "text/html" => new ProductHtmlConverter(fileManagementClient),
            "original" => new ProductJsonConverter(fileManagementClient),
            _ => throw new Exception($"Unsupported file format '{fileFormat}' was passed to ProductConverterFactory")
        };
    }
}
