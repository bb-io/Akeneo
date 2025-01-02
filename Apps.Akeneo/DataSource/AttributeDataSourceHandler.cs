using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class AttributeDataSourceHandler(InvocationContext invocationContext)
    : AkeneoInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new RestRequest("attributes");

        var result = await Client.Paginate<AttributeEntity>(request);
        return result                
            .ToDictionary(x => x.Code, x => x.Code)
            .Where(x => context.SearchString is null ||
                        x.Value.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary();
    }
}