using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class CategoryDataSourceHandler(InvocationContext invocationContext) 
    : AkeneoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        var request = new RestRequest("categories");

        var result = await Client.Paginate<CategoryEntity>(request);
        return result                
            .Where(x => 
                context.SearchString is null ||
                x.CategoryCode.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.CategoryCode, x.CategoryCode));
    }
}
