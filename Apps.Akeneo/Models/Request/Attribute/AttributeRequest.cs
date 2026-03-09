using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Attribute;

public class AttributeRequest
{
    [Display("Attribute code"), DataSource(typeof(AttributeDataSourceHandler))]
    public string AttributeCode { get; set; }
}
