using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Entities;

public class ProductEntity
{
    [Display("Product ID")]
    public string Uuid { get; set; }
    
    public bool Enabled { get; set; }
    
    public string Family { get; set; }
    
    public string? Parent { get; set; }
    
    [Display("Created at")]
    public DateTime Created { get; set; }
    
    [Display("Updated at")]
    public DateTime? Updated { get; set; }
}