using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Request.Product;

public class SearchProductsInCatalogInput
{
    [Display("Updated before")]
    public DateTime? UpdatedBefore { get; set; }
    
    [Display("Updated after")]
    public DateTime? UpdatedAfter { get; set; }
}