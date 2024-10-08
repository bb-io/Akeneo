using Apps.Akeneo.Models.Entities;

namespace Apps.Akeneo.Models.Request.ProductModel
{
    public class UpdateProductModelRequest
    {
        public Dictionary<string, ProductValueEntity[]> Values { get; set; }

        public UpdateProductModelRequest(ProductModelContentEntity productContentEntity)
        {
            Values = productContentEntity.Values;
        }
    }
}
