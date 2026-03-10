using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.ProductModel;
using Blackbird.Applications.Sdk.Common.Files;

namespace Tests.Akeneo;

[TestClass]
public class ProductModelTests : TestBase
{
    private readonly ProductModelActions _actions;

    public ProductModelTests()
    {
        _actions = new ProductModelActions(InvocationContext, FileManager);
    }

    [TestMethod]
    public async Task SearchProductModels_ReturnsProductModels()
    {
        // Arrange
        var input = new SearchProductModelRequest { };
        var locale = new LocaleRequest { Locale = "en_US" };

        // Act
        var result = await _actions.SearchProductModels(input, locale);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetProductModelHtml_IsSuccess()
    {
        // Arrange
        var input = new ProductModelRequest { ProductModelCode = "Milwaukee Mens Black No Days Off Hooded Sweatshirt" };
        var locale = new LocaleRequest { Locale = "de_DE" };

        // Act
        var result = await _actions.GetProductModelHtml(input, locale);

        // Assert
        Console.WriteLine(result.File.Name);
        Assert.IsNotNull(result.File);
    }

    [TestMethod]
    public async Task UpdateProductModelHtml_IsSuccess()
    {
        // Arrange
        var productModel = new ProductModelOptionalRequest { };
        var locale = new LocaleRequest { Locale = "de_DE" };
        var file = new FileModel { File = new FileReference { Name = "test.html" } };

        // Act
        await _actions.UpdateProductModelHtml(productModel, locale, file);
    }
}
