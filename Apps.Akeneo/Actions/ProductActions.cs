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
using Apps.Akeneo.Models.Response.Product;
using Apps.Akeneo.Models.Utility;
using Apps.Akeneo.Services.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.Akeneo.Actions;

[ActionList("Products")]
public class ProductActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AkeneoInvocable(invocationContext)
{
    private readonly ContentServiceFactory _factory = new(invocationContext, fileManagementClient);

    [Action("Search products", Description = "Search for products based on filter criteria")]
    public async Task<ListProductResponse> SearchProducts(
        [ActionParameter] SearchProductsRequest input, 
        [ActionParameter] LocaleRequest locale)
    {
        input.ValidateDates();

        var query = new SearchQuery();
        query.Add("name", new QueryOperator { Operator = "CONTAINS", Value = input.Name, Locale = locale.Locale });
        query.Add("categories", new QueryOperator { Operator = "IN", Value = input.Categories });
        query.Add("enabled", new QueryOperator { Operator = "=", Value = input.Enabled });
        query.AddDateBefore("updated", input.UpdatedBefore);
        query.AddDateAfter("updated", input.UpdatedAfter);
        query.AddDateBefore("created", input.CreatedBefore);
        query.AddDateAfter("created", input.CreatedAfter);

        var request = new RestRequest("products-uuid");
        request.AddQueryParameter("locales", locale.Locale);
        request.AddQueryParameter("search", query.ToString());

        if (input.Attributes != null)
        {
            request.AddQueryParameter("attributes", string.Join(',', input.Attributes.Distinct()));
        }

        var products = await Client.Paginate<ProductEntity>(request);
        
        if (input.Attributes != null && input.AttributeValues != null)
        {
            if (input.Attributes.Count() != input.AttributeValues.Count())
            {
                throw new PluginMisconfigurationException(
                    $"Mismatch between the number of attributes ({input.Attributes.Count()}) and attribute values ({input.AttributeValues.Count()}). Both should have the same number of elements.");
            }

            var zipped = input.Attributes.Zip(input.AttributeValues).ToList();
            foreach (var zippedElement in zipped)
            {
                products = products.Where(x =>
                {
                    var array = x.Values[zippedElement.First]?.ToObject<List<JObject>>()
                                ?? new List<JObject>();
                    
                    var desiredAttribute = array.FirstOrDefault(x => x["locale"]?.ToString() == locale.Locale);
                    if (desiredAttribute == null && array.All(x => string.IsNullOrEmpty(x["locale"]?.ToString())))
                    {
                        desiredAttribute = array.FirstOrDefault();
                    }

                    if (desiredAttribute != null && desiredAttribute["data"]?.ToString() == zippedElement.Second)
                    {
                        return true;
                    }
                    
                    return false;
                }).ToList();
            }
        }

        return new()
        {
            Products = products
        };
    }

    [Action("Get product info", Description = "Get details about a specific product")]
    public Task<ProductEntity> GetProduct([ActionParameter] ProductRequest input)
    {
        var request = new RestRequest($"/products-uuid/{input.ProductId}");
        return Client.ExecuteWithErrorHandling<ProductEntity>(request);
    }

    [Action("Update product info", Description = "Update details of specific product")]
    public Task UpdateProduct([ActionParameter] ProductRequest product, [ActionParameter] UpdateProductInfoInput input)
    {
        var request = new RestRequest($"/products-uuid/{product.ProductId}", Method.Patch)
            .WithJsonBody(input, JsonConfig.Settings);

        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Delete product", Description = "Delete a specific product")]
    public Task DeleteProduct([ActionParameter] ProductRequest input)
    {
        var request = new RestRequest($"/products-uuid/{input.ProductId}", Method.Delete);
        return Client.ExecuteWithErrorHandling(request);
    }

    [Action("Download product content", Description = "Get product content in HTML or JSON format (see docs)")]
    public async Task<FileModel> GetProductHtml(
        [ActionParameter] ProductRequest input, 
        [ActionParameter] LocaleRequest locale, 
        [ActionParameter] OptionalFileTypeHandler fileType,
        [ActionParameter] OptionalChannelRequest channelInput,
        [ActionParameter] DownloadProductRequest downloadInput)
    {
        var service = _factory.GetContentService(ContentTypeConstants.Product);
        var contentInput = new ContentRequest { ContentType = ContentTypeConstants.Product, ContentId = input.ProductId };
        var downloadContentInput = new DownloadContentRequest { IgnoreNonScopable = downloadInput.IgnoreNonScopable };

        var file = await service.DownloadContent(contentInput, locale, channelInput, fileType, downloadContentInput);
        return new FileModel { File = file };
    }

    [Action("Upload product content", Description = "Update product content from a Blackbird generated HTML or JSON file (see docs)")]
    public async Task UpdateProductHtml(
        [ActionParameter] ProductOptionalRequest input,
        [ActionParameter] LocaleRequest locale,
        [ActionParameter] OptionalChannelRequest channelInput,
        [ActionParameter] FileModel file)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(file.File);

        DetectedContent contentData = await ContentTypeDetector.DetectFromFile(fileStream, file.File);
        if (contentData.ContentType != ContentTypeConstants.Product)
            throw new PluginMisconfigurationException(
                $"Product content expected, instead {contentData.ContentType} was provided");

        var service = _factory.GetContentService(ContentTypeConstants.Product);
        await service.UploadContent(input.ProductId, locale.Locale, channelInput.ChannelCode, contentData);
    }
}