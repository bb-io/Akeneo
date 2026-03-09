using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response.Attributes;

public class GetAttributeResponse
{
    public GetAttributeResponse(AttributeEntity entity)
    {
        Code = entity.Code;
        Type = entity.Type;
        Group = entity.Group;
        IsLocalizable = entity.IsLocalizable;
        IsUnique = entity.IsUnique;
        IsScopable = entity.IsScopable;
        AvailableLocales = entity.AvailableLocales;
        Labels = entity.Labels.Select(kvp => new LabelResponse
        {
            Locale = kvp.Key,
            Value = kvp.Value
        }).ToList();
    }

    [Display("Attribute code")]
    public string Code { get; set; }

    [Display("Attribute type")]
    public string Type { get; set; }

    [Display("Attribute group")]
    public string Group { get; set; }

    [Display("Attribute is localizable")]
    public bool IsLocalizable { get; set; }

    [Display("Attribute is unique")]
    public bool IsUnique { get; set; }

    [Display("Attribute is scopable")]
    public bool IsScopable { get; set; }

    [Display("Available locales")]
    public IEnumerable<string> AvailableLocales { get; set; }

    [Display("Labels")]
    public IEnumerable<LabelResponse> Labels { get; set; }
}
