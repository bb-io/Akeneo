﻿using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource
{
    public class CategoryDataSourceHandler : AkeneoInvocable, IAsyncDataSourceItemHandler
    {
        public CategoryDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
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
                .Take(40).Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
