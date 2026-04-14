using Apps.Akeneo.Actions;
using Apps.Akeneo.Constants;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Response.Content;
using Blackbird.Applications.Sdk.Common.Files;
using System.Net.Mime;
using Tests.Akeneo.Base;

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
            ContentId = "9f48bf26-0e07-42f0-bd3d-27a70849efc5"
        };
        var channel = new OptionalChannelRequest { };
        var fileType = new OptionalFileTypeHandler { FileType = MediaTypeNames.Text.Html };
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
            Locale = "fr_FR"
        };
        var channel = new OptionalChannelRequest { ChannelCode = "ecommerce" };

        // Act
        await Actions.UploadContent(uploadRequest, channel);
    }
}
