using Apps.Akeneo.Actions;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.ProductModel;
using Blackbird.Applications.Sdk.Common.Files;
using System.Net.Mime;
using Tests.Akeneo.Base;

namespace Tests.Akeneo;

[TestClass]
public class ProductModelTests : TestBase
{
    private ProductModelActions Actions => new(InvocationContext, FileManager);

    [TestMethod]
    public async Task SearchProductModels_ReturnsProductModels()
    {
        // Arrange
        var input = new SearchProductModelRequest { };
        var locale = new LocaleRequest { Locale = "en_US" };

        // Act
        var result = await Actions.SearchProductModels(input, locale);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task DownloadProductModelContent_IsSuccess()
    {
        // Arrange
        var input = new ProductModelRequest { ProductModelCode = "Cambridge Brown Wicker Outdoor Patio Lounge Chair with CushionGuard Cushions" };
        var locale = new LocaleRequest { Locale = "en_US" };
        var fileType = new OptionalFileTypeHandler { FileType = MediaTypeNames.Application.Json };
        var channel = new OptionalChannelRequest { ChannelCode = "ecommerce" };
        var downloadInput = new DownloadProductModelRequest { IgnoreNonScopable = true };

        // Act
        var result = await Actions.GetProductModelHtml(input, locale, fileType, channel, downloadInput);

        // Assert
        Console.WriteLine(result.File.Name);
        Assert.IsNotNull(result.File);
    }

    [TestMethod]
    public async Task UploadProductModelContent_IsSuccess()
    {
        // Arrange
        var productModel = new ProductModelOptionalRequest { };
        var locale = new LocaleRequest { Locale = "fr_FR" };
        var file = new FileModel { File = new FileReference { Name = "test.json" } };
        var channel = new OptionalChannelRequest { };

        // Act
        await Actions.UpdateProductModelHtml(productModel, locale, channel, file);
    }
}
