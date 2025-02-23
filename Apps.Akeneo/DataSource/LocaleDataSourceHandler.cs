using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class LocaleDataSourceHandler : AkeneoInvocable, IAsyncDataSourceItemHandler
{
    public LocaleDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var query = new SearchQuery();
        query.Add("enabled", new QueryOperator { Operator = "=", Value = true });    

        var request = new RestRequest("locales");
        request.AddQueryParameter("search", query.ToString());

        var result = await Client.Paginate<LocaleEntity>(request);
        return result
            .Where(x => context.SearchString is null ||
                        x.Code.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Code, x.Code));
    }
}