namespace Apps.Akeneo.Polling.Models.Memory;

public class HashMemory
{
    public DateTime LastInteractionDate { get; set; }

    public Dictionary<string, string> ContentHashes { get; set; } = [];
}
