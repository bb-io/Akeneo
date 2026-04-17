using Tests.Akeneo.Base;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Polling;
using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Blackbird.Applications.Sdk.Common.Polling;

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
        var locale = new LocaleRequest { Locale = "de_DE" };

        // Act
        var result = await Polling.OnProductsCreatedOrUpdated(request, filter, locale);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}
