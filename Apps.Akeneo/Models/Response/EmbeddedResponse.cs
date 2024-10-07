namespace Apps.Akeneo.Models.Response;

public class EmbeddedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
}