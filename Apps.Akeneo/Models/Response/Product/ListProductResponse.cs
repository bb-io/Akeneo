using Apps.Akeneo.Models.Entities;

namespace Apps.Akeneo.Models.Response.Product;

public class ListProductResponse
{
    public IEnumerable<ProductEntity> Products { get; set; }
}