using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Polling.Models.Memory;

namespace Apps.Akeneo.Helper;

public static class PollingFilterHelper
{
    public static List<IContentEntity> GetChangedEntities(
        IEnumerable<IContentEntity> fetchedEntities, 
        HashMemory memory,
        string targetLocale)
    {
        var triggeredEntities = new List<IContentEntity>();

        foreach (var entity in fetchedEntities)
        {
            string entityId = entity.Id;
            string currentHash = ContentHashHelper.GenerateContentHash(entity.Values);

            bool isInMemory = memory.ContentHashes.TryGetValue(entityId, out string? previousHash);
            bool isGenuinelyNew = entity.Created >= memory.LastInteractionDate;

            if (isGenuinelyNew)
            {
                if (HasDataForLocale(entity, targetLocale))
                    triggeredEntities.Add(entity);
                
                memory.ContentHashes[entityId] = currentHash;
            }
            else if (isInMemory)
            {
                if (previousHash == currentHash) 
                    continue;
                
                triggeredEntities.Add(entity);
                memory.ContentHashes[entityId] = currentHash;
            }
            else
                memory.ContentHashes[entityId] = currentHash;
        }

        return triggeredEntities;
    }
    
    private static bool HasDataForLocale(IContentEntity entity, string targetLocale)
    {
        if (entity.Values.Count == 0) 
            return false;

        return entity.Values.Values
            .SelectMany(valueArray => valueArray)
            .Any(productValue => string.Equals(productValue.Locale, targetLocale, StringComparison.OrdinalIgnoreCase));
    }
}