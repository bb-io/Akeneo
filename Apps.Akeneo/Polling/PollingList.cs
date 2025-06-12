using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Response.Product;
using Apps.Akeneo.Models.Response.ProductModel;
using Apps.Akeneo.Polling.Models;
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
    
    [PollingEvent("On products created or updated", "This event triggers when any products are created or updated")]
    public async Task<PollingEventResponse<DateMemory, ListProductResponse>> OnProductsCreatedOrUpdated(
        PollingEventRequest<DateMemory> input, [PollingEventParameter] ProductFilter filter)
    {
        if (input.Memory is null)
        {
            return new ()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastInteractionDate = DateTime.UtcNow
                }
            };
        }

        var query = new SearchQuery();

        if (filter.Categories != null && filter.Categories.Any())
        {
            query.Add("categories", new QueryOperator { Operator = "IN", Value = filter.Categories });
        }

        if (filter.Enabled.HasValue)
        {
            query.Add("enabled", new QueryOperator { Operator = "=", Value = filter.Enabled });
        }

        query.Add("updated", new QueryOperator { Operator = ">", Value = input.Memory.LastInteractionDate.ToString("yyyy-MM-dd HH:mm:ss") });

        var request = new RestRequest("products-uuid");
        request.AddQueryParameter("search", query.ToString());

        var products = await Client.Paginate<ProductEntity>(request);

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

    [PollingEvent("On product models created or updated", "This event triggers when any product models are created or updated")]
    public async Task<PollingEventResponse<DateMemory, ListProductModelResponse>> OnProductModelsCreatedOrUpdated(
        PollingEventRequest<DateMemory> input, [PollingEventParameter] ProductModelFilter filter)
    {
        if (input.Memory is null)
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

        var query = new SearchQuery();

        if (filter.Categories != null && filter.Categories.Any())
        {
            query.Add("categories", new QueryOperator { Operator = "IN", Value = filter.Categories });
        }

        query.Add("updated", new QueryOperator { Operator = ">", Value = input.Memory.LastInteractionDate.ToString("yyyy-MM-dd HH:mm:ss") });

        var request = new RestRequest("product-models");
        request.AddQueryParameter("search", query.ToString());

        var models = await Client.Paginate<ProductModelEntity>(request);

        if (!models.Any())
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
                ProductModels = models
            },
            Memory = new()
            {
                LastInteractionDate = DateTime.UtcNow
            }
        };
    }
}