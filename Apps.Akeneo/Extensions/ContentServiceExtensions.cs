using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Services.Content;

namespace Apps.Akeneo.Extensions;

public static class ContentServiceExtensions
{
    public static async Task<IEnumerable<IContentEntity>> ExecuteMany(
        this List<IContentService> contentServices,
        SearchContentRequest searchInput,
        LocaleRequest localeInput)
    {
        var searchTasks = contentServices.Select(service => service.SearchContent(searchInput, localeInput.Locale));
        var results = await Task.WhenAll(searchTasks);

        return results.SelectMany(result => result);
    }
}
