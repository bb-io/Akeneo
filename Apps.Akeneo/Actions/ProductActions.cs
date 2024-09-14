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
    public async Task<ListProductResponse> SearchProducts([ActionParameter] SearchProductsRequest input)
    {
        var searchQuery =
            $"{{\"name\":[{{\"operator\":\"CONTAINS\",\"locale\":\"en_US\",\"value\":\"{input.Name}\"}}]}}";
        var request = new RestRequest($"products-uuid?search={searchQuery}");

        return new()
        {
            Products = await Client.Paginate<ProductEntity>(request)
        };
    }

    [Action("Get product info", Description = "Get details about specific product")]
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

    [Action("Delete product", Description = "Delete specific product")]
    public Task DeleteProduct([ActionParameter] ProductRequest input)
    {
        var request = new RestRequest($"/products-uuid/{input.ProductId}", Method.Delete);
        return Client.ExecuteWithErrorHandling(request);
    }

    #region Html

    [Action("Get product as HTML", Description = "Get product content in HTML format")]
    public async Task<FileModel> GetProductHtml([ActionParameter] ProductRequest input,
        [ActionParameter] LocaleRequest locale)
    {
        var product = await GetProductContent(input.ProductId);

        var htmlStream = ProductHtmlConverter.ToHtml(product, locale.Locale);
        return new()
        {
            File = await _fileManagementClient.UploadAsync(htmlStream, MediaTypeNames.Text.Html, $"{product.Uuid}.html")
        };
    }

    [Action("Update product from HTML", Description = "Update product content from HTML file")]
    public async Task UpdateProductHtml([ActionParameter] ProductRequest input,
        [ActionParameter] LocaleRequest locale, [ActionParameter] FileModel file)
    {
        var fileStream = await _fileManagementClient.DownloadAsync(file.File);
        var product = await GetProductContent(input.ProductId);

        var updatedProduct = ProductHtmlConverter.UpdateFromHtml(product, locale.Locale, fileStream);

        var request = new RestRequest($"/products-uuid/{input.ProductId}", Method.Patch)
            .WithJsonBody(new UpdateProductRequest(updatedProduct), JsonConfig.Settings);

        await Client.ExecuteWithErrorHandling(request);
    }

    #endregion

    private Task<ProductContentEntity> GetProductContent(string productId)
    {
        var request = new RestRequest($"/products-uuid/{productId}");
        return Client.ExecuteWithErrorHandling<ProductContentEntity>(request);
    }
}