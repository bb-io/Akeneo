using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Response.Product;
using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Akeneo.Polling;

[PollingEventList("Products")]
public class ProductPollingList(InvocationContext invocationContext) : AkeneoInvocable(invocationContext)
{
    [PollingEvent("On products created or updated", "This event triggers whenever products are created or updated")]
    public async Task<PollingEventResponse<DateMemory, ListProductResponse>> OnProductsCreatedOrUpdated(
        PollingEventRequest<DateMemory> input, 
        [PollingEventParameter] ProductFilter filter,
        [PollingEventParameter] LocaleRequest localeInput)
    {
        if (input.Memory is null)
            return PollingHelper.NoFlight<ListProductResponse>();

        var query = new SearchQuery();
        query.AddDateAfter("updated", input.Memory.LastInteractionDate);

        if (filter.Categories != null && filter.Categories.Any())
            query.Add("categories", new QueryOperator { Operator = "IN", Value = filter.Categories });

        if (filter.Enabled.HasValue)
            query.Add("enabled", new QueryOperator { Operator = "=", Value = filter.Enabled });

        var request = new RestRequest("products-uuid");

        var search = query.ToString();
        if (string.IsNullOrWhiteSpace(search))
            throw new PluginMisconfigurationException("Search query is empty. Check filters/memory.");

        request.AddQueryParameter("search", query.ToString());
        request.AddQueryParameter("locales", localeInput.Locale);

        var products = await Client.Paginate<ProductEntity>(request);

        if (products.Count == 0)
            return PollingHelper.NoFlight<ListProductResponse>();

        return PollingHelper.TriggerFlight<ListProductResponse>(new(products));
    }
}
