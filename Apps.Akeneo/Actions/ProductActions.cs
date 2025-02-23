using System.Net.Mime;
using Apps.Akeneo.Constants;
using Apps.Akeneo.HtmlConversion;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Response.Product;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;
using Apps.Akeneo.Models.Queries;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Apps.Akeneo.Actions;

[ActionList]
public class ProductActions : AkeneoInvocable
{
    private readonly IFileManagementClient _fileManagementClient;

    public ProductActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
        invocationContext)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Search products", Description = "Search for products based on filter criteria")]
    public async Task<ListProductResponse> SearchProducts([ActionParameter] SearchProductsRequest input, 
        [ActionParameter] LocaleRequest locale)
    {
        var query = new SearchQuery();
        query.Add("name", new QueryOperator { Operator = "CONTAINS", Value = input.Name, Locale = locale.Locale });
        query.Add("categories", new QueryOperator { Operator = "IN", Value = input.Categories });
        query.Add("enabled", new QueryOperator { Operator = "=", Value = input.Enabled });
        query.Add("updated", new QueryOperator { Operator = ">", Value = input.Updated?.ToString("yyyy-MM-dd HH:mm:ss") });

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

    #region Html

    [Action("Download product content", Description = "Get product content in HTML or JSON format (see docs)")]
    public async Task<FileModel> GetProductHtml([ActionParameter] ProductRequest input, [ActionParameter] LocaleRequest locale, [ActionParameter] OptionalFileTypeHandler fileType)
    {     
        if (fileType.FileType == null || fileType.FileType == "html")
        {
            var product = await GetProductContent(input.ProductId);
            var htmlStream = ProductHtmlConverter.ToHtml(product, locale.Locale);
            return new()
            {
                File = await _fileManagementClient.UploadAsync(htmlStream, MediaTypeNames.Text.Html, $"{product.Id}.html")
            };
        }

        var response = await GetProductContentRaw(input.ProductId);
        var jsonBytes = Encoding.UTF8.GetBytes(response.Content);
        var stream = new MemoryStream(jsonBytes);

        return new()
        {
            File = await _fileManagementClient.UploadAsync(stream, MediaTypeNames.Application.Json, $"{input.ProductId.Trim()}.json")
        };        
    }

    [Action("Upload product content", Description = "Update product content from a Blackbird generated HTML or JSON file (see docs)")]
    public async Task UpdateProductHtml([ActionParameter] ProductOptionalRequest input,
        [ActionParameter] LocaleRequest locale, [ActionParameter] FileModel file)
    {
        var fileStream = await _fileManagementClient.DownloadAsync(file.File);
        ProductContentEntity updatedProduct;
        string productId;

        if(file.File.Name.EndsWith("json"))
        {
            var reader = new StreamReader(fileStream);
            var json = await reader.ReadToEndAsync();
            updatedProduct = JsonConvert.DeserializeObject<ProductContentEntity>(json);
            productId = updatedProduct.Id;
        } else
        {
            var htmlDoc = ProductHtmlConverter.LoadHtml(fileStream);
            productId = input.ProductId ?? ProductHtmlConverter.GetResourceId(htmlDoc);
            var product = await GetProductContent(productId);
            updatedProduct = ProductHtmlConverter.UpdateFromHtml(product, locale.Locale, htmlDoc);
        }

        updatedProduct.Values = updatedProduct.Values.Where(x => x.Value.All(y => y.Locale != null && y.Scope != null)).ToDictionary();

        var request = new RestRequest($"/products-uuid/{productId}", Method.Patch)
            .WithJsonBody(new UpdateProductRequest(updatedProduct), JsonConfig.Settings);

        await Client.ExecuteWithErrorHandling(request);
    }

    #endregion

    private Task<ProductContentEntity> GetProductContent(string productId)
    {
        var request = new RestRequest($"/products-uuid/{productId}");
        return Client.ExecuteWithErrorHandling<ProductContentEntity>(request);
    }

    private Task<RestResponse> GetProductContentRaw(string productId)
    {
        var request = new RestRequest($"/products-uuid/{productId}");
        return Client.ExecuteWithErrorHandling(request);
    }
}