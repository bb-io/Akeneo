using Apps.Akeneo.DataSource;
using Apps.Akeneo.DataSource.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Akeneo.Models.Response.Content;

public class UploadContentRequest : IUploadContentInput
{
    [Display("Content")]
    public FileReference Content { get; set; } = null!;

    [Display("Locale"), DataSource(typeof(LocaleDataSourceHandler))]
    public string Locale { get; set; } = string.Empty;

    [Display("Content type"), StaticDataSource(typeof(ContentTypeDataSourceHandler))]
    public string? ContentType { get; set; }

    [Display("Content ID"), DataSource(typeof(ContentDataSourceHandler))]
    public string? ContentId { get; set; }
}
