using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Response.ProductModel;
using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Akeneo.Polling;

[PollingEventList("Product models")]
public class ProductModelPollingList(InvocationContext invocationContext) : AkeneoInvocable(invocationContext)
{
    [PollingEvent("On product models created or updated", "This event triggers whenever product models are created or updated")]
    public async Task<PollingEventResponse<DateMemory, ListProductModelResponse>> OnProductModelsCreatedOrUpdated(
        PollingEventRequest<DateMemory> input, 
        [PollingEventParameter] ProductModelFilter filter)
    {
        if (input.Memory is null)
            return PollingHelper.NoFlight<ListProductModelResponse>();

        var query = new SearchQuery();
        query.AddDateAfter("updated", input.Memory.LastInteractionDate);

        if (filter.Categories != null && filter.Categories.Any())
            query.Add("categories", new QueryOperator { Operator = "IN", Value = filter.Categories });

        var request = new RestRequest("product-models");
        request.AddQueryParameter("search", query.ToString());

        var models = await Client.Paginate<ProductModelEntity>(request);

        if (models.Count == 0)
            return PollingHelper.NoFlight<ListProductModelResponse>();

        return PollingHelper.TriggerFlight<ListProductModelResponse>(new(models));
    }
}
