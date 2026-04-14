using Apps.Akeneo.DataSource;
using Apps.Akeneo.Models.Filter;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Product;

public class SearchProductsRequest : ICreatedDateFilter, IUpdatedDateFilter
{
    public string? Name { get; set; }

    [Display("Categories", Description = "Filter the result by products that contain any of the following categories")]
    [DataSource(typeof(CategoryDataSourceHandler))]
    public IEnumerable<string>? Categories { get; set; }

    [Display("Is enabled?")]
    public bool? Enabled { get; set; }

    [DataSource(typeof(AttributeDataSourceHandler))]
    public IEnumerable<string>? Attributes { get; set; }
    
    [Display("Attribute values")]
    public IEnumerable<string>? AttributeValues { get; set; }

    [Display("Created after")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before")]
    public DateTime? CreatedBefore { get; set; }

    [Display("Updated before")]
    public DateTime? UpdatedBefore { get; set; }

    [Display("Updated after")]
    public DateTime? UpdatedAfter { get; set; }
}