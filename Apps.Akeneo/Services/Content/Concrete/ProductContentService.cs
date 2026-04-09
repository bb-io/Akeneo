using Apps.Akeneo.Constants;
using Apps.Akeneo.Conversion.Product;
using Apps.Akeneo.Extensions;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Response.Content;
using Apps.Akeneo.Models.Utility;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using HtmlAgilityPack;
using RestSharp;
using System.Net.Mime;

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
        LocaleRequest locale,
        OptionalChannelRequest channelInput,
        OptionalFileTypeHandler fileTypeInput,
        DownloadContentRequest downloadInput)
    {
        FileReference fileReference;
        var product = await GetProductContent(input.ContentId);

        switch (fileTypeInput.FileType)
        {
            case null or "text/html":
                var htmlStream = ProductHtmlConverter.ToOutputStream(
                    product,
                    locale.Locale,
                    channelInput.ChannelCode,
                    downloadInput.IgnoreNonScopable ?? false);

                string htmlFileName = input.ContentId.ToFileName("html");
                fileReference = await fileManagementClient.UploadAsync(htmlStream, MediaTypeNames.Text.Html, htmlFileName);
                break;

            case "original":
                var stream = ProductJsonConverter.ToOutputStream(
                    product,
                    locale.Locale,
                    channelInput.ChannelCode,
                    downloadInput.IgnoreNonScopable ?? false);

                string jsonFileName = input.ContentId.ToFileName("json");
                fileReference = await fileManagementClient.UploadAsync(stream, MediaTypeNames.Application.Json, jsonFileName);
                break;

            default:
                throw new PluginMisconfigurationException($"This content type is not supported: {fileTypeInput.FileType}");
        }

        return fileReference;
    }

    public async Task UploadContent(string? contentId, string locale, string? channelInput, DetectedContent detectedContent)
    {
        ProductContentEntity updatedProduct;
        string productId;

        if (detectedContent.Payload is null)
            throw new PluginMisconfigurationException("Deserialized content is null");

        switch (detectedContent.FileFormat)
        {
            case MediaTypeNames.Text.Html:
                var htmlDoc = detectedContent.Payload as HtmlDocument ??
                    throw new PluginMisconfigurationException("Could not convert HTML content to HtmlDoc");

                productId = contentId ?? ProductHtmlConverter.GetResourceId(htmlDoc);
                var product = await GetProductContent(productId);
                updatedProduct = ProductHtmlConverter.UpdateFromHtml(product, locale, htmlDoc, channelInput);
                break;

            case MediaTypeNames.Application.Json:
                string jsonContent = detectedContent.Payload as string ??
                    throw new PluginMisconfigurationException("Could not convert JSON payload to string");

                updatedProduct = ProductJsonConverter.UpdateFromJson<ProductContentEntity>(jsonContent, locale, channelInput);
                productId = contentId ?? updatedProduct.Id;
                break;

            default:
                throw new PluginMisconfigurationException($"This file format is not supported: {detectedContent.ContentType}");
        }        

        updatedProduct.Values = updatedProduct.Values
            .Where(x => x.Value.All(y => y.Locale != null && y.Scope != null))
            .ToDictionary();

        var request = new RestRequest($"/products-uuid/{productId}", Method.Patch)
            .WithJsonBody(new UpdateProductRequest(updatedProduct), JsonConfig.Settings);

        await Client.ExecuteWithErrorHandling(request);
    }

    private async Task<ProductContentEntity> GetProductContent(string productId)
    {
        var request = new RestRequest($"/products-uuid/{productId}");
        return await Client.ExecuteWithErrorHandling<ProductContentEntity>(request);
    }
}
