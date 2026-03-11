using Apps.Akeneo.Actions;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Product;
using Blackbird.Applications.Sdk.Common.Files;
using Tests.Akeneo.Base;

namespace Tests.Akeneo;

[TestClass]
public class Products : TestBase
{
    [TestMethod]
    public async Task GetProductHtml_IsSuccess()
    {
        // Arrange
        var actions = new ProductActions(InvocationContext, FileManager);
        var product = new ProductRequest { ProductId = "005f730c-2e31-49a0-8172-96dc65fd9b20" };
        var locale = new LocaleRequest { Locale = "de_DE" };
        var fileType = new OptionalFileTypeHandler { };
        var channel = new OptionalChannelRequest { };
        var downloadInput = new DownloadProductRequest { IgnoreNonScopable = true };

        // Act
        var result = await actions.GetProductHtml(product, locale, fileType, channel, downloadInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result.File);
    }

    [TestMethod]
    public async Task UpdateProductHtml_IsSuccess()
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

    [TestMethod]
    public async Task Get_product_as_json_works()
    {
        // Arrange
        var productInput = new ProductRequest { ProductId = "005f730c-2e31-49a0-8172-96dc65fd9b20" };
        var locale = new LocaleRequest { Locale = "en_US" };
        var fileType = new OptionalFileTypeHandler { FileType = "json" };
        var channel = new OptionalChannelRequest { };
        var actions = new ProductActions(InvocationContext, FileManager);
        var downloadInput = new DownloadProductRequest { IgnoreNonScopable = true };

        // Act
        var result = await actions.GetProductHtml(productInput, locale, fileType, channel, downloadInput);

        // Assert
        Assert.IsTrue(result.File != null);
    }

    [TestMethod]
    public async Task Update_product_from_json_works()
    {
        // Arrange
        var actions = new ProductActions(InvocationContext, FileManager);
        var fileReference = new FileReference() { Name = "005f730c-2e31-49a0-8172-96dc65fd9b20.json" };
        var file = new FileModel { File = fileReference };
        var productRequest = new ProductOptionalRequest { };
        var localeRequest = new LocaleRequest { Locale = "en_US" };
        var channel = new OptionalChannelRequest { ChannelCode = "b2b" };

        // Act
        await actions.UpdateProductHtml(productRequest, localeRequest, channel, file);
    }
}