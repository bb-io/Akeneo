using Tests.Akeneo.Base;
using Apps.Akeneo.Actions;
using Apps.Akeneo.Constants;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Response.Content;
using Blackbird.Applications.Sdk.Common.Files;

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

    [TestMethod]
    public async Task DownloadContent_IsSuccess()
    {
        // Arrange
        var locale = new LocaleRequest { Locale = "en_US" };
        var content = new ContentRequest 
        { 
            ContentType = ContentTypeConstants.Product, 
            ContentId = "00da5aad-b78f-413f-a927-1b8fdbd0e9a4" 
        };
        var channel = new OptionalChannelRequest { };
        var fileType = new OptionalFileTypeHandler { FileType = "text/html" };
        var input = new DownloadContentRequest { IgnoreNonScopable = true };

        // Act
        var result = await Actions.DownloadContent(locale, content, channel, fileType, input);

        // Assert
        Console.WriteLine(result.Content.Name);
        Assert.IsNotNull(result.Content);
    }

    [TestMethod]
    public async Task UploadContent_IsSuccess()
    {
        // Arrange
        var file = new FileReference { Name = "test.html" };
        var uploadRequest = new UploadContentRequest
        {
            Content = file,
            Locale = "ja_JP"
        };
        var channel = new OptionalChannelRequest { };

        // Act
        await Actions.UploadContent(uploadRequest, channel);
    }
}
