using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Models.Request;
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
        };
        var labelInput = new LabelRequest
        {
            LabelLocales = ["en_US", "de_DE"],
            LabelValues = ["testEN", "testDE"]
        };

        // Act
        var result = await _actions.CreateAttribute(input, labelInput);

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
        };
        var labelInput = new LabelRequest
        {
            LabelLocales = ["nl_NL"],
            LabelValues = ["hello world"],
        };

        // Act
        var result = await _actions.UpdateAttribute(attributeInput, input, labelInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task SearchAttributeOptions_ReturnsAttributeOptions()
    {
        // Arrange
        var attrInput = new AttributeRequest { AttributeCode = "mytest" };

        // Act
        var result = await _actions.SearchAttributeOptions(attrInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task GetAttributeOption_ReturnsAttributeOption()
    {
        // Arrange
        var attrInput = new AttributeRequest { AttributeCode = "mytest" };
        var optionInput = new AttributeOptionRequest { AttributeOptionCode = "my_new_one" };

        // Act
        var result = await _actions.GetAttributeOption(attrInput, optionInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CreateAttributeOption_ReturnsCreatedOption()
    {
        // Arrange
        var attrInput = new AttributeRequest { AttributeCode = "mytest" };
        var input = new CreateAttributeOptionRequest
        {
            AttributeOptionCode = "attr_from_tests2",
        };
        var labelInput = new LabelRequest
        {
            LabelLocales = ["en_US", "de_DE"],
            LabelValues = ["testENG", "testDEU"]
        };

        // Act
        var result = await _actions.CreateAttributeOption(attrInput, input, labelInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateAttributeOption_ReturnsUpdatedOption()
    {
        // Arrange
        var attributeInput = new AttributeRequest { AttributeCode = "mytest" };
        var attrOptionInput = new AttributeOptionRequest { AttributeOptionCode = "attr_from_tests" };
        var labelInput = new LabelRequest 
        { 
            LabelLocales = ["en_US", "nl_NL"], 
            LabelValues = ["my test", "lol"] 
        };

        // Act
        var result = await _actions.UpdateAttributeOption(attributeInput, attrOptionInput, labelInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
