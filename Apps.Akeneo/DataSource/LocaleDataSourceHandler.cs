using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.DataSource;

public class LocaleDataSourceHandler : AkeneoInvocable, IAsyncDataSourceHandler
{
    public LocaleDataSourceHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var searchQuery = "{\"enabled\":[{\"operator\":\"=\",\"value\":true}]}";
        var request = new RestRequest($"locales?search={searchQuery}");

        var result = await Client.Paginate<LocaleEntity>(request);
        return result
            .Where(x => context.SearchString is null ||
                        x.Code.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(40)
            .ToDictionary(x => x.Code, x => x.Code);
    }
}