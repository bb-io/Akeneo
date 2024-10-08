namespace Apps.Akeneo.Models.Entities
{
    public interface IContentEntity
    {
        public string Id { get; set; }
        public Dictionary<string, ProductValueEntity[]> Values { get; set; }
    }
}