using Apps.Akeneo.Constants;
using Apps.Akeneo.Conversion.Product;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Response.Content;
using Apps.Akeneo.Models.Utility;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Akeneo.Services.Content.Concrete;

public class ProductContentService(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : AkeneoInvocable(invocationContext), IContentService
{
    public async Task<SearchContentResponse> SearchContent(SearchContentRequest input, string locale)
    {
        var query = new SearchQuery();
        query.Add("name", new QueryOperator { Operator = "CONTAINS", Value = input.NameContains, Locale = locale });
        query.AddDateAfter("created", input.CreatedAfter);
        query.AddDateAfter("updated", input.UpdatedAfter);
        query.AddDateBefore("created", input.CreatedBefore);
        query.AddDateBefore("updated", input.UpdatedBefore);

        var request = new RestRequest("products-uuid");
        request.AddQueryParameter("locales", locale);
        request.AddQueryParameter("search", query.ToString());

        var products = await Client.Paginate<ProductContentEntity>(request);
        return new(products.Select(x => new GetContentResponse(x, locale)).ToList());
    }

    public async Task<SearchContentResponse> SearchContentMinimal(string locale, string? nameContains)
    {
        var query = new SearchQuery();
        query.Add("name", new QueryOperator { Operator = "CONTAINS", Value = nameContains, Locale = locale });

        var request = new RestRequest("products-uuid");
        request.AddQueryParameter("locales", locale);
        request.AddQueryParameter("search", query.ToString());

        var products = await Client.PaginateOnce<ProductContentEntity>(request);
        return new(products.Select(x => new GetContentResponse(x, locale)).ToList());
    }

    public async Task<FileReference> DownloadContent(
        ContentRequest input,
        string locale,
        string? channelInput,
        string? fileType,
        DownloadContentRequest downloadInput)
    {
        var product = await GetProductContent(input.ContentId);

        var service = ProductConverterFactory.GetConverter(fileType);

        return await service.ToOutputFile(
            product,
            locale, 
            channelInput, 
            downloadInput.IgnoreNonScopable ?? false, 
            fileManagementClient);
    }

    public async Task UploadContent(string? contentId, string locale, string? channelInput, DetectedContent detectedContent)
    {
        var service = ProductConverterFactory.GetConverter(detectedContent.FileFormat);

        var updatedProduct = service.UpdateFromFile(detectedContent.Payload, contentId, locale, channelInput) 
            as ProductContentEntity ??
            throw new PluginMisconfigurationException("Updated content is empty");

        updatedProduct.Values = updatedProduct.Values
            .Where(x => x.Value.All(y => y.Locale != null && y.Scope != null))
            .ToDictionary();

        var request = new RestRequest($"/products-uuid/{updatedProduct.Id}", Method.Patch)
            .WithJsonBody(new UpdateProductRequest(updatedProduct), JsonConfig.Settings);

        await Client.ExecuteWithErrorHandling(request);
    }

    private async Task<ProductContentEntity> GetProductContent(string productId)
    {
        var request = new RestRequest($"/products-uuid/{productId}");
        return await Client.ExecuteWithErrorHandling<ProductContentEntity>(request);
    }
}
