namespace Apps.Akeneo.Models.Request.Product;

public class UpdateProductInfoInput
{
    public bool? Enabled { get; set; }
    
    public string? Family { get; set; }
    
    public string? Parent { get; set; }
}