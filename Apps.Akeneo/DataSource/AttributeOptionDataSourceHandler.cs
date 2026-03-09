using RestSharp;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request.Attribute;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Akeneo.DataSource;

public class AttributeOptionDataSourceHandler(
    InvocationContext invocationContext,
    [ActionParameter] AttributeRequest attributeInput)
    : AkeneoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(attributeInput.AttributeCode))
            throw new PluginMisconfigurationException("Please specify the attribute code first");

        var request = new RestRequest($"attributes/{attributeInput.AttributeCode}/options");
        var response = await Client.Paginate<AttributeOptionEntity>(request);
        return response
            .Where(x => 
                context.SearchString is null ||
                x.OptionCode.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.OptionCode, x.OptionCode)).ToList();
    }
}
