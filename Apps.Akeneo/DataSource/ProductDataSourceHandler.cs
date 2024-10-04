using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
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
        var query = new SearchQuery();

        if (!string.IsNullOrEmpty(context.SearchString))
        {
            query.Add("name", new QueryOperator { Operator = "CONTAINS", Value = context.SearchString, Locale = "en_US" });
        }        

        var request = new RestRequest($"products-uuid");
        request.AddQueryParameter("search", query.ToString());

        var result = await Client.Paginate<ProductContentEntity>(request);
        return result
            .Take(40)
            .ToDictionary(x => x.Uuid,
                x => x.Values["name"].First(x => x.Locale == "en_US").Data.ToString());
    }
}