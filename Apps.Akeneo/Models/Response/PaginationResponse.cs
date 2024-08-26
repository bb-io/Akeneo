using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Response;

public class PaginationResponse<T>
{
    [JsonProperty("_embedded")]
    public EmbeddedResponse<T> Embedded { get; set; }
}