using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class CategoryEntity
{
    [JsonProperty("code")]
    public string CategoryCode { get; set; } = string.Empty;

    [JsonProperty("parent")]
    public string? ParentCategoryCode { get; set; }

    [JsonProperty("updated")]
    public DateTime LastUpdated { get; set; }

    [JsonProperty("labels")]
    public Dictionary<string, string> Labels { get; set; } = [];
}
