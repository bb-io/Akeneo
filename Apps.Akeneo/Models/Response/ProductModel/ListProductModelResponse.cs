using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response.ProductModel;

public record ListProductModelResponse(List<ProductModelEntity> ProductModels) 
{
    [Display("Product models")]
    public List<ProductModelEntity> ProductModels { get; set; } = ProductModels;
}
