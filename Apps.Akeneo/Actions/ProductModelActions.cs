using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Queries;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Request;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using Apps.Akeneo.Models.Request.ProductModel;
using Apps.Akeneo.Models.Response.ProductModel;
using Apps.Akeneo.Constants;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Apps.Akeneo.HtmlConversion;
using Apps.Akeneo.Models;
using System.Net.Mime;

namespace Apps.Akeneo.Actions
{
    [ActionList]
    public class ProductModelActions : AkeneoInvocable
    {
        private readonly IFileManagementClient _fileManagementClient;

        public ProductModelActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(
            invocationContext)
        {
            _fileManagementClient = fileManagementClient;
        }

        [Action("Search product models", Description = "Search for product models based on filter criteria")]
        public async Task<ListProductModelResponse> SearchProductModels([ActionParameter] SearchProductModelRequest input, [ActionParameter] LocaleRequest locale)
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

        #region Html

        [Action("Get product model as HTML", Description = "Get product model content in HTML format")]
        public async Task<FileModel> GetProductModelHtml([ActionParameter] ProductModelRequest input,
            [ActionParameter] LocaleRequest locale)
        {
            var productModel = await GetProductModelContent(input.ProductModelCode);

            var htmlStream = ProductHtmlConverter.ToHtml(productModel, locale.Locale);
            return new()
            {
                File = await _fileManagementClient.UploadAsync(htmlStream, MediaTypeNames.Text.Html, $"{productModel.Code}.html")
            };
        }

        // TODO: We should embed the ID in the HTMl file so that the product input can become optional
        [Action("Update product model from HTML", Description = "Update product model content from HTML file")]
        public async Task UpdateProductModelHtml([ActionParameter] ProductModelRequest input,
            [ActionParameter] LocaleRequest locale, [ActionParameter] FileModel file)
        {
            var fileStream = await _fileManagementClient.DownloadAsync(file.File);
            var productModel = await GetProductModelContent(input.ProductModelCode);

            var updatedProduct = ProductHtmlConverter.UpdateFromHtml(productModel, locale.Locale, fileStream);

            var request = new RestRequest($"/product-models/{input.ProductModelCode}", Method.Patch)
                .WithJsonBody(new UpdateProductModelRequest(updatedProduct), JsonConfig.Settings);

            await Client.ExecuteWithErrorHandling(request);
        }

        #endregion

        private Task<ProductModelContentEntity> GetProductModelContent(string modelCode)
        {
            var request = new RestRequest($"/product-models/{modelCode}");
            return Client.ExecuteWithErrorHandling<ProductModelContentEntity>(request);
        }
    }
}
