using Apps.Akeneo.Models.Entities;

namespace Apps.Akeneo.Models.Request.Product;

public class UpdateProductRequest
{
    public Dictionary<string, ProductValueEntity[]> Values { get; set; }

    public UpdateProductRequest(ProductContentEntity productContentEntity)
    {
        Values = productContentEntity.Values;
    }
}