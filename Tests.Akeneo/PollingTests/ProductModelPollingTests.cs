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
        var request = new PollingEventRequest<DateMemory>
        {
            Memory = new DateMemory { LastInteractionDate = DateTime.UtcNow.AddDays(-1) }
        };
        var input = new ProductModelFilter { };

        // Act
        var result = await Polling.OnProductModelsCreatedOrUpdated(request, input);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
