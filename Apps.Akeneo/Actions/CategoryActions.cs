using Apps.Akeneo.Constants;
using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Category;
using Apps.Akeneo.Models.Response.Category;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Akeneo.Actions;

[ActionList("Categories")]
public class CategoryActions(InvocationContext invocationContext) : AkeneoInvocable(invocationContext)
{
    [Action("Search categories", Description = "Search for categories")]
    public async Task<SearchCategoriesResponse> SearchCategories()
    {
        var request = new RestRequest("categories");
        var response = await Client.Paginate<CategoryEntity>(request);
        return new(response.Select(x => new GetCategoryResponse(x)).ToList());
    }

    [Action("Get category", Description = "Get category by its code")]
    public async Task<GetCategoryResponse> GetCategory([ActionParameter] CategoryRequest categoryInput)
    {
        var request = new RestRequest($"categories/{categoryInput.CategoryCode}");
        var response = await Client.ExecuteWithErrorHandling<CategoryEntity>(request);
        return new(response);
    }

    [Action("Create category", Description = "Create a category")]
    public async Task<GetCategoryResponse> CreateCategory(
        [ActionParameter] CreateCategoryRequest input,
        [ActionParameter] LabelRequest labelInput)
    {
        labelInput.Validate();

        var body = new Dictionary<string, object?>
        {
            { "code", input.CategoryCode },
            { "parent", input.ParentCategoryCode }
        };

        if (labelInput.LabelLocales != null)
        {
            var labelsDict = LabelHelper.GenerateLabelsBody(labelInput.LabelLocales, labelInput.LabelValues!);
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest("categories", Method.Post).WithJsonBody(body, JsonConfig.Settings);
        await Client.ExecuteWithErrorHandling(request);

        return await GetCategory(new() { CategoryCode = input.CategoryCode });
    }

    [Action("Update category", Description = "Update an existing category")]
    public async Task<GetCategoryResponse> UpdateCategory(
        [ActionParameter] CategoryRequest categoryInput,
        [ActionParameter] UpdateCategoryRequest updateInput,
        [ActionParameter] LabelRequest labelInput)
    {
        var body = new Dictionary<string, object?>
        {
            { "parent", updateInput.ParentCategoryCode }
        };

        if (labelInput.LabelLocales != null)
        {
            var labelsDict = LabelHelper.GenerateLabelsBody(labelInput.LabelLocales, labelInput.LabelValues!);
            body.Add("labels", labelsDict);
        }

        var request = new RestRequest($"categories/{categoryInput.CategoryCode}", Method.Patch)
            .WithJsonBody(body, JsonConfig.Settings);
        await Client.ExecuteWithErrorHandling(request);

        return await GetCategory(categoryInput);
    }
}
