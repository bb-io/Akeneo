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

    [PollingEvent("On products created", "On any products are created")]
    public Task<PollingEventResponse<DateMemory, ListProductResponse>> OnProductsCreated(
        PollingEventRequest<DateMemory> request) => HandleProductPolling(request,
        product => product.Created.ToUniversalTime() >= request.Memory?.LastInteractionDate);

    [PollingEvent("On products updated", "On any products are updated")]
    public Task<PollingEventResponse<DateMemory, ListProductResponse>> OnProductsUpdated(
        PollingEventRequest<DateMemory> request) => HandleProductPolling(request,
        product => product.Updated?.ToUniversalTime() >= request.Memory?.LastInteractionDate);

    [PollingEvent("On products added to catalog", "On new products added to the catalog")]
    public async Task<PollingEventResponse<DateMemory, ListProductResponse>> OnProductsAddedToTheCatalog(
        PollingEventRequest<DateMemory> request,
        [PollingEventParameter] CatalogRequest catalog)
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

        var endpoint = $"catalogs/{catalog.CatalogId}/product-uuids";
        var productsRequest = new RestRequest(endpoint);
        var products = (await Client.Paginate<ProductEntity>(productsRequest))
            .Where(x => x.Created.ToUniversalTime() >= request.Memory.LastInteractionDate)
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