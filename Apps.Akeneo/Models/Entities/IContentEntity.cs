namespace Apps.Akeneo.Models.Entities
{
    public interface IContentEntity
    {
        public Dictionary<string, ProductValueEntity[]> Values { get; set; }
    }
}
