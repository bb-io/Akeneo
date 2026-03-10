using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class AttributeEntity
{
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("group")]
    public string Group { get; set; } = string.Empty;

    [JsonProperty("localizable")]
    public bool IsLocalizable { get; set; }

    [JsonProperty("unique")]
    public bool IsUnique { get; set; }

    [JsonProperty("scopable")]
    public bool IsScopable { get; set; }

    [JsonProperty("available_locales")]
    public IEnumerable<string> AvailableLocales { get; set; } = [];

    [JsonProperty("labels")]
    public Dictionary<string, string> Labels { get; set; } = [];
}