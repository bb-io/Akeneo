namespace Apps.Akeneo.Models.Entities;

public class CatalogEntity
{
    public string Code { get; set; }
    public string? Parent { get; set; }
    public DateTime Updated { get; set; }
    public Dictionary<string, string> Labels { get; set; }
}