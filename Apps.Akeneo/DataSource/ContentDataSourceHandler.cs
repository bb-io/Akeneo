using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Services.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Akeneo.DataSource;

public class ContentDataSourceHandler(
    InvocationContext invocationContext,
    [ActionParameter] ContentRequest contentInput,
    [ActionParameter] LocaleRequest localeInput)
    : AkeneoInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private readonly ContentServiceFactory _factory = new(invocationContext, default!);

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(contentInput.ContentType))
            throw new PluginMisconfigurationException("Please specify content type first");

        var service = _factory.GetContentService(contentInput.ContentType);
        var items = await service.SearchContentMinimal(localeInput.Locale, context.SearchString);
        return items.Items.Select(x => new DataSourceItem(x.ContentId, x.ContentName ?? x.ContentId)).ToList();
    }
}
