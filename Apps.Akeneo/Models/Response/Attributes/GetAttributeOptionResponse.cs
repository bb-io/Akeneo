using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response.Attributes;

public class GetAttributeOptionResponse(AttributeOptionEntity entity)
{
    [Display("Attribute option code")]
    public string OptionCode { get; set; } = entity.OptionCode;

    [Display("Attribute")]
    public string Attribute { get; set; } = entity.Attribute;

    [Display("Labels")]
    public IEnumerable<LabelResponse> Labels { get; set; } 
        = entity.Labels.Select(kvp => new LabelResponse(kvp.Key, kvp.Value)).ToList();
}
