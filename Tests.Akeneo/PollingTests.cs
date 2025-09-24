using Apps.Akeneo.Polling;
using Apps.Akeneo.Polling.Models;
using Apps.Akeneo.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.Akeneo.Base;

namespace Tests.Akeneo
{
    [TestClass]
    public class PollingTests : TestBase
    {
        [TestMethod]
        public async Task OnProductCreated_IsSuccess()
        {
            var polling = new PollingList(InvocationContext);

            var request = new PollingEventRequest<DateMemory>
            {
                Memory = new DateMemory
                {
                    LastInteractionDate = DateTime.UtcNow.AddDays(-1)
                }
                //Memory = null
            };

            var result = await polling.OnProductsCreatedOrUpdated(request, new ProductFilter { });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task OnProductModelsCreatedOrUpdated_IsSuccess()
        {
            var polling = new PollingList(InvocationContext);

            var request = new PollingEventRequest<DateMemory>
            {
                Memory = new DateMemory
                {
                    LastInteractionDate = DateTime.UtcNow.AddDays(-1)
                }
                //Memory = null
            };

            var result = await polling.OnProductModelsCreatedOrUpdated(request, new ProductModelFilter { });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }
    }
}
