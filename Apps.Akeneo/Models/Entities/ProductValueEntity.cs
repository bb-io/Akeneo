namespace Apps.Akeneo.Models.Entities;

public class ProductValueEntity
{
    public string? Locale { get; set; }
    
    public object Data { get; set; }
    
    public string Scope { get; set; }
}