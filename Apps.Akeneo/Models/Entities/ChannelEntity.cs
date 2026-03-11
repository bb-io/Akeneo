using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class ChannelEntity
{
    [JsonProperty("code")]
    public string ChannelCode { get; set; } = string.Empty;

    [JsonProperty("labels")]
    public Dictionary<string, string> Labels { get; set; } = [];
}
