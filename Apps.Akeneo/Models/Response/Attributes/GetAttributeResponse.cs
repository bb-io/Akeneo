using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response.Attributes;

public class GetAttributeResponse(AttributeEntity entity)
{
    [Display("Attribute code")]
    public string Code { get; set; } = entity.Code;

    [Display("Attribute type")]
    public string Type { get; set; } = entity.Type;

    [Display("Attribute group")]
    public string Group { get; set; } = entity.Group;

    [Display("Attribute is localizable")]
    public bool IsLocalizable { get; set; } = entity.IsLocalizable;

    [Display("Attribute is unique")]
    public bool IsUnique { get; set; } = entity.IsUnique;

    [Display("Attribute is scopable")]
    public bool IsScopable { get; set; } = entity.IsScopable;

    [Display("Available locales")]
    public IEnumerable<string> AvailableLocales { get; set; } = entity.AvailableLocales;

    [Display("Labels")]
    public IEnumerable<LabelResponse> Labels { get; set; } 
        = entity.Labels.Select(kvp => new LabelResponse(kvp.Key, kvp.Value)).ToList();
}
