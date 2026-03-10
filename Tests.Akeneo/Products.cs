using Tests.Akeneo.Base;
using Apps.Akeneo.Models;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Product;
using Blackbird.Applications.Sdk.Common.Files;

namespace Tests.Akeneo;

[TestClass]
public class Products : TestBase
{
    public const string PRODUCT_ID = "005f730c-2e31-49a0-8172-96dc65fd9b20";
    public const string LOCALE = "en_US";

    [TestMethod]
    public async Task GetProductHtml_IsSuccess()
    {
        // Arrange
        var actions = new ProductActions(InvocationContext, FileManager);
        var product = new ProductRequest { ProductId = "005f730c-2e31-49a0-8172-96dc65fd9b20" };
        var locale = new LocaleRequest { Locale = "de_DE" };
        var fileType = new OptionalFileTypeHandler { };

        // Act
        var result = await actions.GetProductHtml(product, locale, fileType);

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
        var product = new ProductOptionalRequest { };
        var locale = new LocaleRequest { Locale = "ja_JP" };

        // Act
        await actions.UpdateProductHtml(product, locale, new FileModel { File = fileReference });
    }

    [TestMethod]
    public async Task Get_product_as_json_works()
    {
        var actions = new ProductActions(InvocationContext, FileManager);

        var result = await actions.GetProductHtml(new ProductRequest { ProductId = PRODUCT_ID }, new LocaleRequest { Locale = LOCALE }, new OptionalFileTypeHandler { FileType = "json" });

        Assert.IsTrue(result.File != null);
    }

    [TestMethod]
    public async Task Update_product_from_json_works()
    {
        var actions = new ProductActions(InvocationContext, FileManager);

        var fileReference = new FileReference() { Name = "005f730c-2e31-49a0-8172-96dc65fd9b20.json" };
        await actions.UpdateProductHtml(new ProductOptionalRequest { }, new LocaleRequest { Locale = LOCALE }, new FileModel { File = fileReference });
    }
}