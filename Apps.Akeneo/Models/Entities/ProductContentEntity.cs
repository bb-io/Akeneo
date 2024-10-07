namespace Apps.Akeneo.Models.Entities;

public class ProductContentEntity : ProductEntity, IContentEntity
{
    public Dictionary<string, ProductValueEntity[]> Values { get; set; }
}