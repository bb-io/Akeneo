using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities;

public class ProductEntity
{
    [Display("Product ID"),  JsonProperty("uuid")]
    public string Id { get; set; }
    
    public bool Enabled { get; set; }
    
    public string Family { get; set; }
    
    public string? Parent { get; set; }

    public IEnumerable<string> Categories { get; set; }
    public IEnumerable<string> Groups { get; set; }
    
    [Display("Created at")]
    public DateTime Created { get; set; }
    
    [Display("Updated at")]
    public DateTime? Updated { get; set; }
}