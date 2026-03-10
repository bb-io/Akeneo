using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Category;

namespace Tests.Akeneo;

[TestClass]
public class CategoryTests : TestBase
{
    private readonly CategoryActions _actions;

    public CategoryTests()
    {
        _actions = new CategoryActions(InvocationContext);
    }

    [TestMethod]
    public async Task SearchCategories_ReturnsCategories()
    {
		// Arrange


		// Act
        var result = await _actions.SearchCategories();

		// Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
	}

    [TestMethod]
    public async Task GetCategory_ReturnsCategory()
    {
        // Arrange
        var categoryInput = new CategoryRequest { CategoryCode = "imported_products" };

        // Act
        var result = await _actions.GetCategory(categoryInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task CreateCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var input = new CreateCategoryRequest
        {
            CategoryCode = "mytest213child",
            ParentCategoryCode = "mytest213"
        };
        var labelInput = new LabelRequest
        {
            LabelLocales = ["en_US", "it_IT"],
            LabelValues = ["hi", "bro"]
        };

        // Act
        var result = await _actions.CreateCategory(input, labelInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateCategory_ReturnsUpdatedCategory()
    {
        // Arrange
        var categoryInput = new CategoryRequest { CategoryCode = "mytest213child" };
        var updateInput = new UpdateCategoryRequest { ParentCategoryCode = "master" };
        var labelsInput = new LabelRequest
        {
            LabelLocales = ["fr_FR"],
            LabelValues = ["Master catalogue FR"]
        };

        // Act
        var result = await _actions.UpdateCategory(categoryInput, updateInput, labelsInput);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
