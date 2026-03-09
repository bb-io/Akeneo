using Apps.Akeneo.DataSource;
using Apps.Akeneo.DataSource.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Akeneo.Models.Request.Attribute;

public class CreateAttributeRequest
{
    [Display("Attribute code")]
    public string AttributeCode { get; set; }

    [Display("Attribute type"), StaticDataSource(typeof(AttributeTypeDataSourceHandler))]
    public string AttributeType { get; set; }

    [Display("Attribute group"), DataSource(typeof(AttributeGroupDataSourceHandler))]
    public string AttributeGroup { get; set; }

    [Display("Attribute is localizable")]
    public bool IsLocalizable { get; set; }

    [Display("Attribute is scopable")]
    public bool IsScopable { get; set; }

    [Display("Attribute is unique")]
    public bool IsUnique { get; set; }
}
