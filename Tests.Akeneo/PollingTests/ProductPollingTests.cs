using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Blackbird.Applications.Sdk.Common.Polling;
using Apps.Akeneo.Polling;
using Tests.Akeneo.Base;

namespace Tests.Akeneo.PollingTests;

[TestClass]
public class ProductPollingTests : TestBase
{
    private ProductPollingList Polling => new(InvocationContext);

    [TestMethod]
    public async Task OnProductCreated_ReturnsProducts()
    {
        // Arrange
        var request = new PollingEventRequest<DateMemory>
        {
            Memory = new DateMemory { LastInteractionDate = DateTime.UtcNow.AddDays(-1) }
        };
        var filter = new ProductFilter { };

        // Act
        var result = await Polling.OnProductsCreatedOrUpdated(request, filter);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
