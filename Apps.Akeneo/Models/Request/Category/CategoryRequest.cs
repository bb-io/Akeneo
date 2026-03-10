using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Category;

public class CategoryRequest
{
    [Display("Category code"), DataSource(typeof(CategoryDataSourceHandler))]
    public string CategoryCode { get; set; }
}
