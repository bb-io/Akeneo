using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Product;

public class ProductOptionalRequest
{
    [Display("Product ID")]
    [DataSource(typeof(ProductDataSourceHandler))]
    public string? ProductId { get; set; }
}