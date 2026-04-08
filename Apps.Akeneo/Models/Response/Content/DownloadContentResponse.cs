using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Akeneo.Models.Response.Content;

public record DownloadContentResponse(FileReference FileReference) : IDownloadContentOutput
{
    [Display("Content")]
    public FileReference Content { get; set; } = FileReference;
}
