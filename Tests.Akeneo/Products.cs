using Apps.Akeneo.Actions;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Product;
using Blackbird.Applications.Sdk.Common.Files;
using System.Net.Mime;
using Tests.Akeneo.Base;

namespace Tests.Akeneo;

[TestClass]
public class Products : TestBase
{
    [TestMethod]
    public async Task DownloadProductContent_IsSuccess()
    {
        // Arrange
        var actions = new ProductActions(InvocationContext, FileManager);
        var product = new ProductRequest { ProductId = "005f730c-2e31-49a0-8172-96dc65fd9b20" };
        var locale = new LocaleRequest { Locale = "en_US" };
        var fileType = new OptionalFileTypeHandler { FileType = MediaTypeNames.Application.Json };
        var channel = new OptionalChannelRequest { };
        var downloadInput = new DownloadProductRequest { IgnoreNonScopable = true };

        // Act
        var result = await actions.GetProductHtml(product, locale, fileType, channel, downloadInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result.File);
    }

    [TestMethod]
    public async Task UploadProductContent_IsSuccess()
    {
        // Arrange
        var actions = new ProductActions(InvocationContext, FileManager);
        var fileReference = new FileReference() { Name = "test.html" };
        var file = new FileModel { File = fileReference };
        var product = new ProductOptionalRequest { };
        var locale = new LocaleRequest { Locale = "de_DE" };
        var channel = new OptionalChannelRequest { ChannelCode = "b2b" };

        // Act
        await actions.UpdateProductHtml(product, locale, channel, file);
    }
}