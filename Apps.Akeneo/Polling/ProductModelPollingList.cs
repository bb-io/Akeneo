using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request;
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
    public async Task<PollingEventResponse<HashMemory, ListProductModelResponse>> OnProductModelsCreatedOrUpdated(
        PollingEventRequest<HashMemory> input, 
        [PollingEventParameter] ProductModelFilter filter,
        [PollingEventParameter] LocaleRequest localeInput)
    {
        if (input.Memory is null)
            return PollingHelper.NoFlight<ListProductModelResponse>(input.Memory);

        var query = new SearchQuery();
        query.AddDateAfter("updated", input.Memory.LastInteractionDate);

        if (filter.Categories != null && filter.Categories.Any())
            query.Add("categories", new QueryOperator { Operator = "IN", Value = filter.Categories });

        var request = new RestRequest("product-models");
        request.AddQueryParameter("search", query.ToString());
        request.AddQueryParameter("locales", localeInput.Locale);

        var models = await Client.Paginate<ProductModelContentEntity>(request);
        var triggeredModels = new List<ProductModelEntity>();

        foreach (var model in models)
        {
            string modelId = model.Id;
            string currentHash = ContentHashHelper.GenerateContentHash(model.Values);

            bool isBrandNewProduct = !input.Memory.ContentHashes.TryGetValue(modelId, out string? previousHash);
            if (isBrandNewProduct || previousHash != currentHash)
            {
                triggeredModels.Add(model);
                input.Memory.ContentHashes[modelId] = currentHash;
            }
        }

        if (triggeredModels.Count == 0)
            return PollingHelper.NoFlight<ListProductModelResponse>(input.Memory);

        return PollingHelper.TriggerFlight<ListProductModelResponse>(new(triggeredModels), input.Memory);
    }
}
