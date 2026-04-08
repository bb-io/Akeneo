using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Constants;

namespace Tests.Akeneo;

[TestClass]
public class ContentTests : TestBase
{
    private ContentActions Actions => new(InvocationContext, FileManager);

    [TestMethod]
    public async Task SearchContent_ReturnsContent()
    {
        // Arrange
        var contentTypesInput = new ContentTypesRequest { ContentTypes = [ContentTypeConstants.ProductModel] };
        var searchInput = new SearchContentRequest
        {
            NameContains = "Whitfield 5-Piece Dark Brown Metal Outdoor Patio Round Fire Pit"
        };
        var localeInput = new LocaleRequest { Locale = "en_US" };

		// Act
        var result = await Actions.SearchContent(contentTypesInput, searchInput, localeInput);

		// Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
	}
}
