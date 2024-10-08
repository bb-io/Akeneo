namespace Apps.Akeneo.Models.Entities
{
    public class ProductModelContentEntity : ProductModelEntity, IContentEntity
    {
        public Dictionary<string, ProductValueEntity[]> Values { get; set; }
    }
}
