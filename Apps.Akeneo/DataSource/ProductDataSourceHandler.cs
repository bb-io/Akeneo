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
        var request = new RestRequest("products-uuid");
        
        if (!string.IsNullOrEmpty(context.SearchString))
        {
            var query = new SearchQuery();
            query.Add("name", new QueryOperator { Operator = "CONTAINS", Value = context.SearchString, Locale = "en_US" });
            request.AddQueryParameter("search", query.ToString());
        }  

        var result = await Client.PaginateOnce<ProductContentEntity>(request);
        return result.ToDictionary(x => x.Uuid, x => x.Values["name"].First(x => x.Locale == "en_US").Data.ToString() ?? "No label for en_US");
    }
}