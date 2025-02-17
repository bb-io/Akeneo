using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource
{
    public class ProductModelDataSourceHandler : AkeneoInvocable, IAsyncDataSourceItemHandler
    {
        public ProductModelDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
            CancellationToken cancellationToken)
        {
            var request = new RestRequest("product-models");

            if (!string.IsNullOrEmpty(context.SearchString))
            {
                var query = new SearchQuery();
                query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = context.SearchString, Locale = "en_US" });
                request.AddQueryParameter("search", query.ToString());
            }

            var result = await Client.PaginateOnce<ProductModelEntity>(request);
            return result.Select(x => new DataSourceItem(x.Id, x.Id));
        }
    }
}
