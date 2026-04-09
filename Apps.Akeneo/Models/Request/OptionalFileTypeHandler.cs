using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.SDK.Blueprints.Handlers;

namespace Apps.Akeneo.Models.Request;

public class OptionalFileTypeHandler
{
    [Display("File format"), StaticDataSource(typeof(DownloadFileFormatHandler))]
    public string? FileType { get; set; }
}
