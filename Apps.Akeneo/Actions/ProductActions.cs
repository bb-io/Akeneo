using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Response.Product;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Akeneo.Actions;

[ActionList]
public class ProductActions : AkeneoInvocable
{
    public ProductActions(InvocationContext invocationContext) : base(invocationContext)
    {
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
    
    [Action("Delete product", Description = "Delete specific product")]
    public Task DeleteProduct([ActionParameter] ProductRequest input)
    {
        var request = new RestRequest($"/products-uuid/{input.ProductId}", Method.Delete);
        return Client.ExecuteWithErrorHandling(request);
    }
}