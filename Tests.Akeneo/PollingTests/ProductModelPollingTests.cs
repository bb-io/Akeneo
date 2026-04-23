using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Polling;
using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.Akeneo.Base;

namespace Tests.Akeneo.PollingTests;

[TestClass]
public class ProductModelPollingTests : TestBase
{
    private ProductModelPollingList Polling => new(InvocationContext);

    [TestMethod]
    public async Task OnProductModelsCreatedOrUpdated_ReturnsProductModels()
    {
        // Arrange
        var request = new PollingEventRequest<HashMemory>
        {
            Memory = new HashMemory { ContentHashes = [] }
        };
        var locale = new LocaleRequest { Locale = "de_DE" };
        var input = new ProductModelFilter { };

        // Act
        var result = await Polling.OnProductModelsCreatedOrUpdated(request, input, locale);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
