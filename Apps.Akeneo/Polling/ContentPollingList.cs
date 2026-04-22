using Apps.Akeneo.Extensions;
using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Polling.Models.Memory;
using Apps.Akeneo.Polling.Models.Request;
using Apps.Akeneo.Polling.Models.Response;
using Apps.Akeneo.Services.Content;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Blackbird.Applications.SDK.Blueprints;

namespace Apps.Akeneo.Polling;

[PollingEventList("Content")]
public class ContentPollingList(InvocationContext invocationContext) : AkeneoInvocable(invocationContext)
{
    private readonly ContentServiceFactory _factory = new(invocationContext, default!);

    [BlueprintEventDefinition(BlueprintEvent.ContentCreatedOrUpdatedMultiple)]
    [PollingEvent("On content created or updated", 
        "This event triggers whenever a content of an existing item is updated or a new item with content is created")]
    public async Task<PollingEventResponse<HashMemory, OnContentCreatedOrUpdatedResponse>> OnContentCreatedOrUpdated(
        PollingEventRequest<HashMemory> input,
        [PollingEventParameter] ContentTypesRequest contentTypesInput,
        [PollingEventParameter] ContentFilter filter,
        [PollingEventParameter] LocaleRequest localeInput)
    {
        if (input.Memory is null)
            return PollingHelper.NoFlight<OnContentCreatedOrUpdatedResponse>(input.Memory);
        
        contentTypesInput.ApplyDefaultValues();
        var services = _factory.GetContentServices(contentTypesInput.ContentTypes!);

        var searchInput = new SearchContentRequest
        {
            UpdatedAfter = input.Memory.LastInteractionDate,
            NameContains = filter.NameContains,
        };
        
        var results = await services.ExecuteMany(searchInput, localeInput);
        var triggeredModels = PollingFilterHelper.GetChangedEntities(results, input.Memory, localeInput.Locale);

        if (triggeredModels.Count == 0)
            return PollingHelper.NoFlight<OnContentCreatedOrUpdatedResponse>(input.Memory);

        var castedResults = triggeredModels.CastToEntities(localeInput.Locale).ToList();
        return PollingHelper.TriggerFlight<OnContentCreatedOrUpdatedResponse>(new(castedResults), input.Memory);
    }
}
