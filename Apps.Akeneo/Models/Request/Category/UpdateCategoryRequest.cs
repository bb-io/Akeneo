using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Category;

public class UpdateCategoryRequest
{
    [Display("Parent category code"), DataSource(typeof(CategoryDataSourceHandler))]
    public string? ParentCategoryCode { get; set; }
}
