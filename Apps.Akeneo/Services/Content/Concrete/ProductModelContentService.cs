using Apps.Akeneo.Constants;
using Apps.Akeneo.Conversion.Product;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Request.ProductModel;
using Apps.Akeneo.Models.Utility;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Akeneo.Services.Content.Concrete;

public class ProductModelContentService(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : AkeneoInvocable(invocationContext), IContentService
{
    public async Task<IEnumerable<IContentEntity>> SearchContent(SearchContentRequest input, string locale)
    {
        var query = new SearchQuery();
        query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = input.NameContains, Locale = locale });
        query.AddDateAfter("created", input.CreatedAfter);
        query.AddDateAfter("updated", input.UpdatedAfter);
        query.AddDateBefore("created", input.CreatedBefore);
        query.AddDateBefore("updated", input.UpdatedBefore);

        var request = new RestRequest("product-models");
        request.AddQueryParameter("locales", locale);
        request.AddQueryParameter("search", query.ToString());

        return await Client.Paginate<ProductModelContentEntity>(request);
    }

    public async Task<IEnumerable<IContentEntity>> SearchContentMinimal(string locale, string? nameContains)
    {
        var query = new SearchQuery();
        query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = nameContains, Locale = locale });

        var request = new RestRequest("product-models");
        request.AddQueryParameter("locales", locale);
        request.AddQueryParameter("search", query.ToString());

        return await Client.PaginateOnce<ProductModelContentEntity>(request);
    }

    public async Task<FileReference> DownloadContent(
        ContentRequest input,
        string locale,
        string? channelInput,
        string? fileType,
        DownloadContentRequest downloadInput)
    {
        var productModel = await GetProductModelContent(input.ContentId);

        var service = ProductConverterFactory.GetConverter(fileType);

        return await service.ToOutputFile(
            productModel,
            locale,
            channelInput,
            downloadInput.IgnoreNonScopable ?? false,
            fileManagementClient);
    }

    public async Task UploadContent(string? contentId, string locale, string? channelInput, DetectedContent detectedContent)
    {
        var service = ProductConverterFactory.GetConverter(detectedContent.FileFormat);

        var updatedProduct = service.UpdateFromFile<ProductModelContentEntity>(
            detectedContent.Payload, 
            contentId, 
            locale, 
            channelInput) ??
            throw new PluginMisconfigurationException("Updated content is empty");

        if (detectedContent.Payload is null)
            throw new PluginMisconfigurationException("Deserialized content is null");

        updatedProduct.Values = updatedProduct.Values
            .Where(x => x.Value.All(y => y.Locale != null && y.Scope != null))
            .ToDictionary();

        var request = new RestRequest($"/product-models/{updatedProduct.Id}", Method.Patch)
            .WithJsonBody(new UpdateProductModelRequest(updatedProduct), JsonConfig.Settings);

        await Client.ExecuteWithErrorHandling(request);
    }

    private async Task<ProductModelContentEntity> GetProductModelContent(string modelCode)
    {
        var request = new RestRequest($"/product-models/{modelCode}");
        return await Client.ExecuteWithErrorHandling<ProductModelContentEntity>(request);
    }
}
