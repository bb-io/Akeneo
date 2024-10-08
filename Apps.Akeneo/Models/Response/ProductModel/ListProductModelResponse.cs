using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response.ProductModel
{
    public class ListProductModelResponse
    {
        [Display("Product models")]
        public IEnumerable<ProductModelEntity> ProductModels { get; set; }
    }
}
