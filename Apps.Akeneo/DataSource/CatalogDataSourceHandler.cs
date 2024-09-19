using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class CatalogDataSourceHandler : AkeneoInvocable, IAsyncDataSourceHandler
{
    public CatalogDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var searchQuery =
            $"{{\"name\":[{{\"operator\":\"CONTAINS\",\"value\":\"{context.SearchString}\"}}], \"enabled\":[{{\"operator\":\"=\",\"value\":true}}]}}";
        var request = new RestRequest($"catalogs?search={searchQuery}");

        var result = await Client.Paginate<CatalogEntity>(request);

        return result
            .Take(40)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}