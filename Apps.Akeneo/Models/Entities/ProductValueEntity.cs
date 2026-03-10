using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class ProductValueEntity
{
    [JsonProperty("locale", NullValueHandling = NullValueHandling.Include)]
    public string? Locale { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }

    [JsonProperty("scope", NullValueHandling = NullValueHandling.Include)]
    public string Scope { get; set; }
}