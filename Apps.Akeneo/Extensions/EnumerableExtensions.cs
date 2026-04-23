using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Response.Content;

namespace Apps.Akeneo.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<GetContentResponse> CastToEntities(this IEnumerable<IContentEntity> entities, string locale)
    {
        return entities.Select(entity => entity switch
        {
            ProductContentEntity product => new GetContentResponse(product, locale),
            ProductModelEntity model => new GetContentResponse(model),
            _ => throw new Exception($"Unknown content entity type: {entity.GetType().Name}")
        });
    }
}