using Apps.Akeneo.Constants;
using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Request.ProductModel;
using Apps.Akeneo.Models.Response.ProductModel;
using Apps.Akeneo.Models.Utility;
using Apps.Akeneo.Services.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.Akeneo.Actions;

[ActionList("Product models")]
public class ProductModelActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AkeneoInvocable(invocationContext)
{
    private readonly ContentServiceFactory _factory = new(invocationContext, fileManagementClient);

    [Action("Search product models", Description = "Search for product models based on filter criteria")]
    public async Task<ListProductModelResponse> SearchProductModels(
        [ActionParameter] SearchProductModelRequest input, 
        [ActionParameter] LocaleRequest locale)
    {
        input.ValidateDates();

        var query = new SearchQuery();
        query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = input.Code, Locale = locale.Locale });
        query.Add("categories", new QueryOperator { Operator = "IN", Value = input.Categories });
        query.AddDateBefore("updated", input.UpdatedBefore);
        query.AddDateAfter("updated", input.UpdatedAfter);
        query.AddDateBefore("created", input.CreatedBefore);
        query.AddDateAfter("created", input.CreatedAfter);

        var request = new RestRequest("product-models");
        request.AddQueryParameter("locales", locale.Locale);
        request.AddQueryParameter("search", query.ToString());

        var productModels = await Client.Paginate<ProductModelEntity>(request);
        return new(productModels);
    }

    [Action("Get product model info", Description = "Get details about a specific product model")]
    public Task<ProductModelEntity> GetProductModel([ActionParameter] ProductModelRequest input)
    {
        var request = new RestRequest($"/product-models/{input.ProductModelCode}");
        return Client.ExecuteWithErrorHandling<ProductModelEntity>(request);
    }

    [Action("Update product model info", Description = "Update details of specific product model")]
    public Task UpdateProductModel([ActionParameter] ProductModelRequest productModel, [ActionParameter] UpdateProductInfoInput input)
    {
        var request = new RestRequest($"/product-models/{productModel.ProductModelCode}", Method.Patch)
            .WithJsonBody(input, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Delete product model", Description = "Delete a specific product model")]
    public Task DeleteProductModel([ActionParameter] ProductModelRequest input)
    {
        var request = new RestRequest($"/product-models/{input.ProductModelCode}", Method.Delete);
        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Download product model content", Description = "Download product model content to a file")]
    public async Task<FileModel> GetProductModelHtml(
        [ActionParameter] ProductModelRequest input,
        [ActionParameter] LocaleRequest locale,
        [ActionParameter] OptionalFileTypeHandler fileType,
        [ActionParameter] OptionalChannelRequest channelInput,
        [ActionParameter] DownloadProductModelRequest downloadInput)
    {
        var service = _factory.GetContentService(ContentTypeConstants.ProductModel);
        var contentInput = new ContentRequest { ContentType = ContentTypeConstants.Product, ContentId = input.ProductModelCode };
        var downloadContentInput = new DownloadContentRequest { IgnoreNonScopable = downloadInput.IgnoreNonScopable };

        var file = await service.DownloadContent(contentInput, locale, channelInput, fileType, downloadContentInput);
        return new FileModel { File = file };
    }

    [Action("Upload product model content", Description = "Upload product model content from a file")]
    public async Task UpdateProductModelHtml(
        [ActionParameter] ProductModelOptionalRequest input,
        [ActionParameter] LocaleRequest locale,
        [ActionParameter] OptionalChannelRequest channelInput,
        [ActionParameter] FileModel file)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(file.File);

        DetectedContent contentData = await ContentTypeDetector.DetectFromFile(fileStream, file.File);
        if (contentData.ContentType != ContentTypeConstants.ProductModel)
            throw new PluginMisconfigurationException(
                $"Product model content expected, instead {contentData.ContentType} was provided");

        var service = _factory.GetContentService(ContentTypeConstants.ProductModel);
        await service.UploadContent(input.ProductModelCode, locale.Locale, channelInput.ChannelCode, contentData);
    }

    [Action("DEBUG: Get auth data", Description = "Can be used only for debugging purposes.")]
    public List<AuthenticationCredentialsProvider> GetAuthenticationCredentialsProviders()
    {
        return InvocationContext.AuthenticationCredentialsProviders.ToList();
    }
}