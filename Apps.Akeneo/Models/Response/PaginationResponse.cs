using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Response;

public class PaginationResponse<T>
{
    [JsonProperty("_embedded")]
    public EmbeddedResponse<T> Embedded { get; set; }    
    
    [JsonProperty("_links")]
    public LinksPaginationResponse Links { get; set; }
    
    public string Error { get; set; }
}