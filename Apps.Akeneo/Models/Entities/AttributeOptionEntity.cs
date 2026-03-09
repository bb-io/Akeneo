using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class AttributeOptionEntity
{
    [JsonProperty("code")]
    public string AttributeCode { get; set; } = string.Empty;

    [JsonProperty("attribute")]
    public string Attribute { get; set; } = string.Empty;

    [JsonProperty("labels")]
    public Dictionary<string, string> Labels { get; set; } = [];
}
