using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response.Category;

public class GetCategoryResponse(CategoryEntity entity)
{
    [Display("Category code")]
    public string CategoryCode { get; set; } = entity.CategoryCode;

    [Display("Parent category code")]
    public string? ParentCategoryCode { get; set; } = entity.ParentCategoryCode;

    [Display("Last updated")]
    public DateTime LastUpdated { get; set; } = entity.LastUpdated;

    [Display("Labels")]
    public IEnumerable<LabelResponse> Labels { get; set; } 
        = entity.Labels.Select(kvp => new LabelResponse(kvp.Key, kvp.Value)).ToList();
}
