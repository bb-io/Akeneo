namespace Apps.Akeneo.Models.Entities;

public class ProductContentEntity : ProductEntity
{
    public Dictionary<string, ProductValueEntity[]> Values { get; set; }
}