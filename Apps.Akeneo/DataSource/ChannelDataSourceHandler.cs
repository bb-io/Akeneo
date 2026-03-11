using RestSharp;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Akeneo.DataSource;

public class ChannelDataSourceHandler(InvocationContext invocationContext)
    : AkeneoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        var request = new RestRequest("channels");
        var response = await Client.PaginateOnce<ChannelEntity>(request);

        return response
            .Where(x =>
                context.SearchString is null ||
                x.ChannelCode.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.ChannelCode, x.ChannelCode))
            .ToList();
    }
}
