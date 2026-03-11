using Tests.Akeneo.Base;
using Apps.Akeneo.DataSource;
using Apps.Akeneo.Models.Request.Attribute;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Tests.Akeneo;

[TestClass]
public class DataSource : TestBase
{
    [TestMethod]
    public async Task Product_data_handler_works()
    {
        var handler = new ProductDataSourceHandler(InvocationContext);

        var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

        Console.WriteLine($"Total: {result.Count()}");
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value}: {item.DisplayName}");
        }

        Assert.IsTrue(result.Count() > 0);
    }

    [TestMethod]
    public async Task AttributeDataSourceHandler_ReturnsAttributes()
    {
        // Arrange
        var handler = new AttributeDataSourceHandler(InvocationContext);

        // Act
        var result = await handler.GetDataAsync(new(), default);

        // Assert
        PrintDataHandlerResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task AttributeGroupDataSourceHandler_ReturnsAttributeGroups()
    {
        // Arrange
        var handler = new AttributeGroupDataSourceHandler(InvocationContext);

        // Act
        var result = await handler.GetDataAsync(new() { SearchString = "other" }, default);

        // Assert
        PrintDataHandlerResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task AttributeOptionDataSourceHandler_ReturnsAttributeOptions()
    {
        // Arrange
        var attributeInput = new AttributeRequest { AttributeCode = "mytest" };
        var handler = new AttributeOptionDataSourceHandler(InvocationContext, attributeInput);

        // Act
        var result = await handler.GetDataAsync(new(), default);

        // Assert
        PrintDataHandlerResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task LocaleDataSourceHandler_ReturnsLocales()
    {
        // Arrange
        var handler = new LocaleDataSourceHandler(InvocationContext);

        // Act
        var result = await handler.GetDataAsync(new(), default);

        // Assert
        PrintDataHandlerResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CategoryDataSourceHandler_ReturnsCategories()
    {
        // Arrange
        var handler = new CategoryDataSourceHandler(InvocationContext);

        // Act
        var result = await handler.GetDataAsync(new(), default);

        // Assert
        PrintDataHandlerResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ChannelDataSourceHandler_ReturnsChannels()
    {
        // Arrange
        var handler = new ChannelDataSourceHandler(InvocationContext);

        // Act
        var result = await handler.GetDataAsync(new(), default);

        // Assert
        PrintDataHandlerResult(result);
        Assert.IsNotNull(result);
    }
}
