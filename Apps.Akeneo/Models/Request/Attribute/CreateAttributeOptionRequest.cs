using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Request.Attribute;

public class CreateAttributeOptionRequest
{
    [Display("Attribute option code")]
    public string AttributeOptionCode { get; set; }
}
