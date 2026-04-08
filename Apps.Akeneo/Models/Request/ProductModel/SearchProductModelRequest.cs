using Apps.Akeneo.DataSource;
using Apps.Akeneo.Models.Filter;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.ProductModel;

public class SearchProductModelRequest : ICreatedDateFilter, IUpdatedDateFilter
{
    [Display("Code contains")]
    public string? Code { get; set; }

    [Display("Categories", Description = "Filter the result by product models that contain any of the following categories")]
    [DataSource(typeof(CategoryDataSourceHandler))]
    public IEnumerable<string>? Categories { get; set; }

    [Display("Created after")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before")]
    public DateTime? CreatedBefore { get; set; }

    [Display("Updated after")]
    public DateTime? UpdatedAfter { get; set; }

    [Display("Updated before")]
    public DateTime? UpdatedBefore { get; set; }
}
