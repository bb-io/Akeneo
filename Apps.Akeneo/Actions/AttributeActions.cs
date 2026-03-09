using RestSharp;
using Apps.Akeneo.Constants;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request;
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
    public async Task<GetAttributeResponse> CreateAttribute(
        [ActionParameter] CreateAttributeRequest input,
        [ActionParameter] LabelRequest labelInput)
    {
        labelInput.Validate();

        var body = new Dictionary<string, object>
        {
            { "code", input.AttributeCode },
            { "type", input.AttributeType },
            { "group", input.AttributeGroup },
            { "localizable", input.IsLocalizable },
            { "scopable", input.IsScopable },
            { "unique", input.IsUnique },
        };

        if (labelInput.LabelLocales != null)
        {
            var labelsDict = GenerateLabelsBody(labelInput.LabelLocales, labelInput.LabelValues!);
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest($"attributes/{input.AttributeCode}", Method.Patch).WithJsonBody(body);
        await Client.ExecuteWithErrorHandling(request);

        return await GetAttribute(new() { AttributeCode = input.AttributeCode });
    }

    [Action("Update attribute", Description = "Update an existing attribute")]
    public async Task<GetAttributeResponse> UpdateAttribute(
        [ActionParameter] AttributeRequest attributeInput,
        [ActionParameter] UpdateAttributeRequest input,
        [ActionParameter] LabelRequest labelInput)
    {
        labelInput.Validate();

        var body = new Dictionary<string, object?>
        {
            { "group", input.AttributeGroup },
        };

        if (labelInput.LabelLocales != null)
        {
            var labelsDict = GenerateLabelsBody(labelInput.LabelLocales, labelInput.LabelValues!);
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest($"attributes/{attributeInput.AttributeCode}", Method.Patch)
            .WithJsonBody(body, JsonConfig.Settings);
        await Client.ExecuteWithErrorHandling(request);

        return await GetAttribute(attributeInput);
    }

    [Action("Search attribute options", Description = "Search for attribute options")]
    public async Task<SearchAttributeOptionsResponse> SearchAttributeOptions(
        [ActionParameter] AttributeRequest attributeInput)
    {
        var request = new RestRequest($"attributes/{attributeInput.AttributeCode}/options");
        var response = await Client.Paginate<AttributeOptionEntity>(request);
        return new(response.Select(x => new GetAttributeOptionResponse(x)).ToList());
    }

    [Action("Get attribute option", Description = "Get attribute option by attribute code")]
    public async Task<GetAttributeOptionResponse> GetAttributeOption(
        [ActionParameter] AttributeRequest attributeInput,
        [ActionParameter] AttributeOptionRequest optionInput)
    {
        var request = new RestRequest($"attributes/{attributeInput.AttributeCode}/options/{optionInput.AttributeOptionCode}");
        var response = await Client.ExecuteWithErrorHandling<AttributeOptionEntity>(request);
        return new(response);
    }

    [Action("Create attribute option", Description = "Create an attribute option")]
    public async Task<GetAttributeOptionResponse> CreateAttributeOption(
        [ActionParameter] AttributeRequest attributeInput,
        [ActionParameter] CreateAttributeOptionRequest input,
        [ActionParameter] LabelRequest labelInput)
    {
        var body = new Dictionary<string, object>
        {
            { "code", input.AttributeOptionCode }
        };

        if (labelInput.LabelLocales != null)
        {
            var labelsDict = GenerateLabelsBody(labelInput.LabelLocales, labelInput.LabelValues!);
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest(
            $"attributes/{attributeInput.AttributeCode}/options/{input.AttributeOptionCode}", 
            Method.Patch)
            .WithJsonBody(body);
        await Client.ExecuteWithErrorHandling(request);

        return await GetAttributeOption(attributeInput, new() { AttributeOptionCode = input.AttributeOptionCode });
    }

    [Action("Update attribute option", Description = "Update an existing attribute option")]
    public async Task<GetAttributeOptionResponse> UpdateAttributeOption(
        [ActionParameter] AttributeRequest attributeInput,
        [ActionParameter] AttributeOptionRequest optionInput,
        [ActionParameter] LabelRequest labelInput)
    {
        labelInput.Validate();

        var body = new Dictionary<string, object>
        {
            { "code", optionInput.AttributeOptionCode }
        };

        if (labelInput.LabelLocales != null)
        {
            var labelsDict = GenerateLabelsBody(labelInput.LabelLocales, labelInput.LabelValues!);
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest(
            $"attributes/{attributeInput.AttributeCode}/options/{optionInput.AttributeOptionCode}",
            Method.Patch)
            .WithJsonBody(body, JsonConfig.Settings);
        await Client.ExecuteWithErrorHandling(request);

        return await GetAttributeOption(attributeInput, optionInput);
    }

    private static Dictionary<string, string> GenerateLabelsBody(List<string> locales, List<string> values)
    {
        var labelsDict = new Dictionary<string, string>();

        for (int i = 0; i < locales.Count; i++)
        {
            string locale = locales[i];
            string value = values[i];
            labelsDict.Add(locale, value);
        }

        return labelsDict;
    }
}
