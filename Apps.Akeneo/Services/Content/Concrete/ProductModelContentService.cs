using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Response.Content;
using Apps.Akeneo.Services.Content.Models;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Akeneo.Services.Content.Concrete;

public class ProductModelContentService(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : AkeneoInvocable(invocationContext), IContentService
{
    public async Task<SearchContentResponse> SearchContent(SearchContentServiceInput input)
    {
        var query = new SearchQuery();
        query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = input.NameContains, Locale = input.Locale });
        query.AddDateAfter("created", input.CreatedAfter);
        query.AddDateAfter("updated", input.UpdatedAfter);
        query.AddDateBefore("created", input.CreatedBefore);
        query.AddDateBefore("updated", input.UpdatedBefore);

        var request = new RestRequest("product-models");
        request.AddQueryParameter("locales", input.Locale);
        request.AddQueryParameter("search", query.ToString());

        var products = await Client.Paginate<ProductModelEntity>(request);
        return new(products.Select(x => new GetContentResponse(x)).ToList());
    }
}
