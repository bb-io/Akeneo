using RestSharp;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Akeneo.DataSource;

public class AttributeGroupDataSourceHandler(InvocationContext invocationContext)
    : AkeneoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        var request = new RestRequest("attribute-groups");
        var result = await Client.Paginate<AttributeGroupEntity>(request);

        return result
            .Where(x => 
                context.SearchString is null || 
                x.AttributeGroupCode.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.AttributeGroupCode, x.AttributeGroupCode))
            .ToList();
    }
}
