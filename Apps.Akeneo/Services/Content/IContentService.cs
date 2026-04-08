using Apps.Akeneo.Models.Response.Content;
using Apps.Akeneo.Services.Content.Models;

namespace Apps.Akeneo.Services.Content;

public interface IContentService
{
    Task<SearchContentResponse> SearchContent(SearchContentServiceInput input);
}
