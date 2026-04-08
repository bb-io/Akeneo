using Apps.Akeneo.Constants;
using Apps.Akeneo.Extensions;
using Apps.Akeneo.HtmlConversion;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Response.Content;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;
using System.Text;

namespace Apps.Akeneo.Services.Content.Concrete;

public class ProductModelContentService(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : AkeneoInvocable(invocationContext), IContentService
{
    public async Task<SearchContentResponse> SearchContent(SearchContentRequest input, string locale)
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

        var products = await Client.Paginate<ProductModelEntity>(request);
        return new(products.Select(x => new GetContentResponse(x)).ToList());
    }

    public async Task<SearchContentResponse> SearchContentMinimal(string locale, string? nameContains)
    {
        var query = new SearchQuery();
        query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = nameContains, Locale = locale });

        var request = new RestRequest("product-models");
        request.AddQueryParameter("locales", locale);
        request.AddQueryParameter("search", query.ToString());

        var products = await Client.PaginateOnce<ProductModelEntity>(request);
        return new(products.Select(x => new GetContentResponse(x)).ToList());
    }

    public async Task<FileReference> DownloadContent(
        ContentRequest input,
        LocaleRequest locale,
        OptionalChannelRequest channelInput,
        OptionalFileTypeHandler fileTypeInput,
        DownloadContentRequest downloadInput)
    {
        FileReference fileReference;

        switch (fileTypeInput.FileType)
        {
            case null or "text/html":
                var productModel = await GetProductModelContent(input.ContentId);

                var htmlStream = ProductHtmlConverter.ToHtml(
                    productModel,
                    locale.Locale,
                    channelInput.ChannelCode,
                    downloadInput.IgnoreNonScopable ?? false,
                    ContentTypeConstants.ProductModel);

                string fileName = productModel.Id.ToFileName("html");
                fileReference = await fileManagementClient.UploadAsync(htmlStream, MediaTypeNames.Text.Html, fileName);
                break;

            case "original":
                var response = await GetProductModelContentRaw(input.ContentId);
                var jsonBytes = Encoding.UTF8.GetBytes(response.Content!);
                var stream = new MemoryStream(jsonBytes);

                string jsonFileName = input.ContentId.ToFileName("json");
                fileReference = await fileManagementClient.UploadAsync(stream, MediaTypeNames.Application.Json, jsonFileName);
                break;

            default:
                throw new PluginMisconfigurationException($"This content type is not supported: {fileTypeInput.FileType}");
        }

        return fileReference;
    }

    private async Task<ProductModelContentEntity> GetProductModelContent(string modelCode)
    {
        var request = new RestRequest($"/product-models/{modelCode}");
        return await Client.ExecuteWithErrorHandling<ProductModelContentEntity>(request);
    }

    private async Task<RestResponse> GetProductModelContentRaw(string modelCode)
    {
        var request = new RestRequest($"/product-models/{modelCode}");
        return await Client.ExecuteWithErrorHandling(request);
    }
}
