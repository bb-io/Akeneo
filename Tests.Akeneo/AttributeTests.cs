using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Models.Request.Attribute;

namespace Tests.Akeneo;

[TestClass]
public class AttributeTests : TestBase
{
    private readonly AttributeActions _actions;

    public AttributeTests()
    {
        _actions = new(InvocationContext);
    }

    [TestMethod]
    public async Task SearchAttributes_ReturnsAttributes()
    {
		// Arrange


		// Act
        var result = await _actions.SearchAttributes();

		// Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetAttribute_ReturnsAttribute()
    {
        // Arrange
        var attributeCode = new AttributeRequest { AttributeCode = "123" };

        // Act
        var result = await _actions.GetAttribute(attributeCode);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CreateAttribute_ReturnsCreatedAttribute()
    {
        // Arrange
        var input = new CreateAttributeRequest
        {
            AttributeCode = "mytest123",
            AttributeType = "pim_catalog_simpleselect",
            AttributeGroup = "other",
            IsLocalizable = true,
            IsScopable = false,
            IsUnique = false,
            LabelLocales = ["en_US", "de_DE"],
            LabelValues = ["testEN", "testDE"]
        };

        // Act
        var result = await _actions.CreateAttribute(input);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateAttribute_ReturnsUpdatedAttribute()
    {
        // Arrange
        var attributeInput = new AttributeRequest { AttributeCode = "mytest123" };
        var input = new UpdateAttributeRequest
        {
            AttributeGroup = "new_group",
            LabelLocales = ["nl_NL"],
            LabelValues = ["hello world"],
        };

        // Act
        var result = await _actions.UpdateAttribute(attributeInput, input);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
