using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Product;

public class UpdateProductInfoInput
{    
    [DataSource(typeof(FamilyDataSourceHandler))]
    public string? Family { get; set; }
    
    [DataSource(typeof(ProductModelDataSourceHandler))]
    public string? Parent { get; set; }
}