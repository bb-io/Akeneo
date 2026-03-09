using RestSharp;
using Apps.Akeneo.Constants;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request.Attribute;
using Apps.Akeneo.Models.Response.Attributes;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;

namespace Apps.Akeneo.Actions;

[ActionList("Attributes")]
public class AttributeActions(InvocationContext invocationContext) : AkeneoInvocable(invocationContext)
{
    [Action("Search attributes", Description = "Search for attributes")]
    public async Task<SearchAttributesResponse> SearchAttributes()
    {
        var request = new RestRequest("attributes");
        var result = await Client.Paginate<AttributeEntity>(request);
        return new(result.Select(x => new GetAttributeResponse(x)).ToList());
    }

    [Action("Get attribute", Description = "Get attribute by its code")]
    public async Task<GetAttributeResponse> GetAttribute([ActionParameter] AttributeRequest attributeRequest)
    {
        var request = new RestRequest($"attributes/{attributeRequest.AttributeCode}");
        var result = await Client.ExecuteWithErrorHandling<AttributeEntity>(request);
        return new(result);
    }

    [Action("Create attribute", Description = "Create an attribute")]
    public async Task<GetAttributeResponse> CreateAttribute([ActionParameter] CreateAttributeRequest input)
    {
        input.Validate();

        var body = new Dictionary<string, object>
        {
            { "code", input.AttributeCode },
            { "type", input.AttributeType },
            { "group", input.AttributeGroup },
            { "localizable", input.IsLocalizable },
            { "scopable", input.IsScopable },
            { "unique", input.IsUnique },
        };

        if (input.LabelLocales != null)
        {
            var labelsDict = new Dictionary<string, string>();

            for (int i = 0; i < input.LabelLocales.Count; i++)
            {
                string locale = input.LabelLocales[i];
                string value = input.LabelValues![i];
                labelsDict.Add(locale, value);
            }
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest($"attributes/{input.AttributeCode}", Method.Patch).WithJsonBody(body);
        await Client.ExecuteWithErrorHandling(request);

        var createdAttrRequest = new RestRequest($"attributes/{input.AttributeCode}");
        var createdAttrResponse = await Client.ExecuteWithErrorHandling<AttributeEntity>(createdAttrRequest);
        return new(createdAttrResponse);
    }

    [Action("Update attribute", Description = "Update an existing attribute")]
    public async Task<GetAttributeResponse> UpdateAttribute(
        [ActionParameter] AttributeRequest attributeInput,
        [ActionParameter] UpdateAttributeRequest input)
    {
        input.Validate();

        var body = new Dictionary<string, object?>
        {
            { "group", input.AttributeGroup },
        };

        if (input.LabelLocales != null)
        {
            var labelsDict = new Dictionary<string, string>();

            for (int i = 0; i < input.LabelLocales.Count; i++)
            {
                string locale = input.LabelLocales[i];
                string value = input.LabelValues![i];
                labelsDict.Add(locale, value);
            }

            body.Add("labels", labelsDict);
        }

        var request = new RestRequest($"attributes/{attributeInput.AttributeCode}", Method.Patch)
            .WithJsonBody(body, JsonConfig.Settings);
        await Client.ExecuteWithErrorHandling(request);

        var updatedAttrRequest = new RestRequest($"attributes/{attributeInput.AttributeCode}");
        var updatedAttrResponse = await Client.ExecuteWithErrorHandling<AttributeEntity>(updatedAttrRequest);
        return new(updatedAttrResponse);
    }
}
