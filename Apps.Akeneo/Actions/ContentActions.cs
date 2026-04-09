using Apps.Akeneo.Extensions;
using Apps.Akeneo.Helper;
using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Channel;
using Apps.Akeneo.Models.Request.Content;
using Apps.Akeneo.Models.Response.Content;
using Apps.Akeneo.Models.Utility;
using Apps.Akeneo.Services.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Blueprints;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Akeneo.Actions;

[ActionList("Content")]
public class ContentActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
    : AkeneoInvocable(invocationContext)
{
    private readonly ContentServiceFactory _factory = new(invocationContext, fileManagementClient);

    [BlueprintActionDefinition(BlueprintAction.SearchContent)]
    [Action("Search content", Description = "Search for multiple types of content")]
    public async Task<SearchContentResponse> SearchContent(
        [ActionParameter] ContentTypesRequest contentTypesInput,
        [ActionParameter] SearchContentRequest searchInput,
        [ActionParameter] LocaleRequest localeInput)
    {
        contentTypesInput.ApplyDefaultValues();
        searchInput.ValidateDates();

        var services = _factory.GetContentServices(contentTypesInput.ContentTypes!);
        return await services.ExecuteMany(searchInput, localeInput);
    }

    [BlueprintActionDefinition(BlueprintAction.DownloadContent)]
    [Action("Download content", Description = "Download content")]
    public async Task<DownloadContentResponse> DownloadContent(
        [ActionParameter] LocaleRequest locale,
        [ActionParameter] ContentRequest input,
        [ActionParameter] OptionalChannelRequest channelInput,
        [ActionParameter] OptionalFileTypeHandler fileTypeInput,
        [ActionParameter] DownloadContentRequest downloadInput)
    {
        var service = _factory.GetContentService(input.ContentType);
        var file = await service.DownloadContent(input, locale, channelInput, fileTypeInput, downloadInput);
        return new(file);
    }

    [BlueprintActionDefinition(BlueprintAction.UploadContent)]
    [Action("Upload content", Description = "Upload content from a file")]
    public async Task UploadContent(
        [ActionParameter] UploadContentRequest uploadInput,
        [ActionParameter] OptionalChannelRequest channelInput)
    {
        using var fileStream = await fileManagementClient.DownloadAsync(uploadInput.Content);

        DetectedContent contentData = await ContentTypeDetector.DetectFromFile(fileStream, uploadInput.Content);
        string contentType = uploadInput.ContentType ?? contentData.ContentType;

        var service = _factory.GetContentService(contentType);
        await service.UploadContent(uploadInput.ContentId, uploadInput.Locale, channelInput.ChannelCode, contentData);
    }
}
