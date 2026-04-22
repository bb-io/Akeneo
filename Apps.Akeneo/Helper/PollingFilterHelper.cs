using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Polling.Models.Memory;

namespace Apps.Akeneo.Helper;

public static class PollingFilterHelper
{
    public static List<IContentEntity> GetChangedEntities(
        IEnumerable<IContentEntity> fetchedEntities, 
        HashMemory memory)
    {
        var triggeredEntities = new List<IContentEntity>();

        foreach (var entity in fetchedEntities)
        {
            string entityId = entity.Id;
            string currentHash = ContentHashHelper.GenerateContentHash(entity.Values);

            bool isInMemory = memory.ContentHashes.TryGetValue(entityId, out string? previousHash);
            
            if (isInMemory)
            {
                if (previousHash == currentHash) 
                    continue;
                
                triggeredEntities.Add(entity);
                memory.ContentHashes[entityId] = currentHash;
            }
            else 
            {
                if (entity.Values.Count > 0)
                    triggeredEntities.Add(entity);
                
                memory.ContentHashes[entityId] = currentHash;
            }
        }

        return triggeredEntities;
    }
}