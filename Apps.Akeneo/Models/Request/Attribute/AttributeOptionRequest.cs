using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Attribute;

public class AttributeOptionRequest
{
    [Display("Attribute option"), DataSource(typeof(AttributeOptionDataSourceHandler))]
    public string AttributeOptionCode { get; set; }
}
