using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Polling;
using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.Akeneo.Base;

namespace Tests.Akeneo.PollingTests;

[TestClass]
public class ContentPollingTests : TestBase
{
    private ContentPollingList Polling => new(InvocationContext);

    [TestMethod]
    public async Task OnContentCreatedOrUpdated_ReturnsContent()
    {
        // Arrange
        var request = new PollingEventRequest<HashMemory>
        {
            Memory = new HashMemory { ContentHashes = [] }
        };
        var contentTypes = new ContentTypesRequest { ContentTypes = [] };
        var filter = new ContentFilter { };
        var locale = new LocaleRequest { Locale = "fr_FR" };

        // Act
        var result = await Polling.OnContentCreatedOrUpdated(request, contentTypes, filter, locale);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task OnContentCreated_ReturnsContent()
    {
        // Arrange
        var request = new PollingEventRequest<DateMemory>
        {
            Memory = new DateMemory { LastInteractionDate = DateTime.UtcNow - TimeSpan.FromHours(1) }
        };
        var contentTypes = new ContentTypesRequest { ContentTypes = [] };

        // Act
        var result = await Polling.OnContentCreated(request, contentTypes);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
