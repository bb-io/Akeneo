using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Product;

public class SearchProductsRequest
{
    public string? Name { get; set; }

    [Display("Categories", Description = "Filter the result by products that are in all of the following categories")]
    [DataSource(typeof(CategoryDataSourceHandler))]
    public IEnumerable<string>? Categories { get; set; }

    public bool? Enabled { get; set; }

    [Display("Updated after")]
    public DateTime? Updated { get; set; }
}