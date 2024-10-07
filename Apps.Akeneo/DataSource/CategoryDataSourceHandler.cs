using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.DataSource
{
    public class CategoryDataSourceHandler : AkeneoInvocable, IAsyncDataSourceHandler
    {
        public CategoryDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
            CancellationToken cancellationToken)
        {
            var query = new SearchQuery();

            var request = new RestRequest("categories");

            var result = await Client.Paginate<CatalogEntity>(request);
            return result                
                .ToDictionary(x => x.Code, x =>
                {
                    if (x.Labels.TryGetValue("en_US", out var label))
                    {
                        return label;
                    }
                    return string.Empty;
                })
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .Where(x => context.SearchString is null ||
                            x.Value.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(40).ToDictionary();
        }
    }
}
