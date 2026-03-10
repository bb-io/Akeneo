using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Attribute;

public class UpdateAttributeRequest
{
    [Display("Attribute group"), DataSource(typeof(AttributeGroupDataSourceHandler))]
    public string? AttributeGroup { get; set; }
}
