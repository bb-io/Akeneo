using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class FamilyDataSourceHandler : AkeneoInvocable, IAsyncDataSourceHandler
{
    public FamilyDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new RestRequest("families");

        if (!string.IsNullOrEmpty(context.SearchString))
        {
            var query = new SearchQuery();
            query.Add("code", new QueryOperator { Operator = "CONTAINS", Value = context.SearchString });
            request.AddQueryParameter("search", query.ToString());
        }

        var result = await Client.PaginateOnce<FamilyEntity>(request);
        return result.ToDictionary(x => x.Code, GetFamilyName);
    }

    private string GetFamilyName(FamilyEntity familyEntity)
    {
        KeyValuePair<string, string>? label = familyEntity.Labels.FirstOrDefault();
        return label == null ? familyEntity.Code : label.Value.Value;
    }
}