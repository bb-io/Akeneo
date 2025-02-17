using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class AttributeDataSourceHandler(InvocationContext invocationContext)
    : AkeneoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("attributes");

        var result = await Client.Paginate<AttributeEntity>(request);
        return result.Where(x => context.SearchString is null || x.Code.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)).Select(x => new DataSourceItem(x.Code, x.Code));
    }
}