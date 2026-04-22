using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Utility;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Akeneo.Services.Content;

public interface IContentService
{
    Task<IEnumerable<IContentEntity>> SearchContent(SearchContentRequest input, string locale);
    Task<IEnumerable<IContentEntity>> SearchContentMinimal(string locale, string? nameContains);
    Task<FileReference> DownloadContent(
        ContentRequest input, 
        string locale,
        string? channelInput,
        string? fileType,
        DownloadContentRequest downloadInput);
    Task UploadContent(string? contentId, string locale, string? channelInput, DetectedContent detectedContent);
}
