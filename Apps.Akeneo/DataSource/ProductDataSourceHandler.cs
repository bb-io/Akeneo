using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class ProductDataSourceHandler : AkeneoInvocable, IAsyncDataSourceHandler
{
    public ProductDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var searchQuery =
            $"{{\"name\":[{{\"operator\":\"CONTAINS\",\"locale\":\"en_US\",\"value\":\"{context.SearchString}\"}}]}}";
        var request = new RestRequest($"products-uuid?search={searchQuery}");

        var result = await Client.Paginate<ProductContentEntity>(request);
        return result
            .Take(40)
            .ToDictionary(x => x.Uuid,
                x => x.Values["name"].First(x => x.Locale == "en_US").Data.ToString());
    }
}