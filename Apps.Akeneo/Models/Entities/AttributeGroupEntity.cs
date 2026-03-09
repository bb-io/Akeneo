using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class AttributeGroupEntity
{
    [JsonProperty("code")]
    public string AttributeGroupCode { get; set; }
}
