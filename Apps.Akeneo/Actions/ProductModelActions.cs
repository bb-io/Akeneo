using Apps.Akeneo.Constants;
using Apps.Akeneo.Helper;
using Apps.Akeneo.HtmlConversion;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Request.ProductModel;
using Apps.Akeneo.Models.Response.ProductModel;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;

namespace Apps.Akeneo.Actions;

[ActionList("Product models")]
public class ProductModelActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AkeneoInvocable(invocationContext)
{
    [Action("Search product models", Description = "Search for product models based on filter criteria")]
    public async Task<ListProductModelResponse> SearchProductModels(
        [ActionParameter] SearchProductModelRequest input, 
        [ActionParameter] LocaleRequest locale)
    {
        var query = new SearchQuery();
        query.Add("identifier", new QueryOperator { Operator = "CONTAINS", Value = input.Code, Locale = locale.Locale });
        query.Add("categories", new QueryOperator { Operator = "IN", Value = input.Categories });
        query.Add("updated", new QueryOperator { Operator = ">", Value = input.Updated?.ToString("yyyy-MM-dd HH:mm:ss") });

        var request = new RestRequest("product-models");
        request.AddQueryParameter("locales", locale.Locale);
        request.AddQueryParameter("search", query.ToString());

        return new()
        {
            ProductModels = await Client.Paginate<ProductModelEntity>(request)
        };
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

    [Action("Download product model content", Description = "Get product model content")]
    public async Task<FileModel> GetProductModelHtml(
        [ActionParameter] ProductModelRequest input,
        [ActionParameter] LocaleRequest locale,
        [ActionParameter] OptionalChannelRequest channelInput)
    {
        var productModel = await GetProductModelContent(input.ProductModelCode);

        var htmlStream = ProductHtmlConverter.ToHtml(productModel, locale.Locale, channelInput.ChannelCode);
        var file = await fileManagementClient.UploadAsync(htmlStream, MediaTypeNames.Text.Html, $"{productModel.Id}.html");
        return new(file);
    }

    [Action("Upload product model content", Description = "Update product model content from a file")]
    public async Task UpdateProductModelHtml(
        [ActionParameter] ProductModelOptionalRequest input,
        [ActionParameter] LocaleRequest locale,
        [ActionParameter] OptionalChannelRequest channelInput,
        [ActionParameter] FileModel file)
    {
        var fileStream = await fileManagementClient.DownloadAsync(file.File);
        var htmlDoc = await ContentDownloader.GetHtmlFromFile(fileStream);

        var productId = input.ProductModelCode ?? ProductHtmlConverter.GetResourceId(htmlDoc);
        var productModel = await GetProductModelContent(productId);

        var updatedProduct = ProductHtmlConverter.UpdateFromHtml(
            productModel, 
            locale.Locale, 
            htmlDoc, 
            channelInput.ChannelCode);

        var request = new RestRequest($"/product-models/{productId}", Method.Patch)
            .WithJsonBody(new UpdateProductModelRequest(updatedProduct), JsonConfig.Settings);

        await Client.ExecuteWithErrorHandling(request);
    }

    private Task<ProductModelContentEntity> GetProductModelContent(string modelCode)
    {
        var request = new RestRequest($"/product-models/{modelCode}");
        return Client.ExecuteWithErrorHandling<ProductModelContentEntity>(request);
    }

    [Action("DEBUG: Get auth data", Description = "Can be used only for debugging purposes.")]
    public List<AuthenticationCredentialsProvider> GetAuthenticationCredentialsProviders()
    {
        return InvocationContext.AuthenticationCredentialsProviders.ToList();
    }
}