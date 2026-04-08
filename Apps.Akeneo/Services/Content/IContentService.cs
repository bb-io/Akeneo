using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Response.Content;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Akeneo.Services.Content;

public interface IContentService
{
    Task<SearchContentResponse> SearchContent(SearchContentRequest input, string locale);
    Task<SearchContentResponse> SearchContentMinimal(string locale, string? nameContains);
    Task<FileReference> DownloadContent(
        ContentRequest input, 
        LocaleRequest locale,
        OptionalChannelRequest channelInput,
        OptionalFileTypeHandler fileTypeInput,
        DownloadContentRequest downloadInput);
}
