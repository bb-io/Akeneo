using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Response.Product;
using Apps.Akeneo.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Akeneo.Polling;

[PollingEventList]
public class PollingList : AkeneoInvocable
{
    public PollingList(InvocationContext invocationContext) : base(invocationContext)
    {
    }
    
    // TODO: Add optional search filters starting with 'category' and 'locales'
    // TODO: Add "On product models created or updated"
    // TODO: Add "On categories created or updated"

    [PollingEvent("On products created or updated", "This event triggers when any products are created or updated")]
    public Task<PollingEventResponse<DateMemory, ListProductResponse>> OnProductsCreated(
        PollingEventRequest<DateMemory> request) => HandleProductPolling(request,
        product => product.Created.ToUniversalTime() >= request.Memory?.LastInteractionDate || product.Updated?.ToUniversalTime() >= request.Memory?.LastInteractionDate);

    private async Task<PollingEventResponse<DateMemory, ListProductResponse>> HandleProductPolling(
        PollingEventRequest<DateMemory> request, Func<ProductEntity, bool> filter)
    {
        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastInteractionDate = DateTime.UtcNow
                }
            };
        }

        var productsRequest = new RestRequest("products-uuid");

        // TODO: https://api.akeneo.com/documentation/filter.html#by-update-date-3 <- use this instead of fetching ALL products
        var products = (await Client.Paginate<ProductEntity>(productsRequest))
            .Where(filter)
            .ToArray();

        if (!products.Any())
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastInteractionDate = DateTime.UtcNow
                }
            };
        }

        return new()
        {
            FlyBird = true,
            Result = new()
            {
                Products = products
            },
            Memory = new()
            {
                LastInteractionDate = DateTime.UtcNow
            }
        };
    }
}