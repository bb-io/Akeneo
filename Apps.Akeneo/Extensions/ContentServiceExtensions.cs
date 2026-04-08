using Apps.Akeneo.Models.Response.Content;
using Apps.Akeneo.Services.Content;
using Apps.Akeneo.Services.Content.Models;

namespace Apps.Akeneo.Extensions;

public static class ContentServiceExtensions
{
    public static async Task<SearchContentResponse> ExecuteMany(
        this List<IContentService> contentServices,
        SearchContentServiceInput request)
    {
        var searchTasks = contentServices.Select(service => service.SearchContent(request));
        var results = await Task.WhenAll(searchTasks);

        var allItems = results.SelectMany(x => x.Items).ToList();
        return new(allItems);
    }
}
