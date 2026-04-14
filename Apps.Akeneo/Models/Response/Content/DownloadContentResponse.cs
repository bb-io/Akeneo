using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Akeneo.Models.Response.Content;

public class DownloadContentResponse(FileReference fileReference) : IDownloadContentOutput
{
    [Display("Content")]
    public FileReference Content { get; set; } = fileReference;
}
