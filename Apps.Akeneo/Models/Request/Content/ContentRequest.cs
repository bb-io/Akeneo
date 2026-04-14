using Apps.Akeneo.DataSource;
using Apps.Akeneo.DataSource.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Akeneo.Models.Request.Content;

public class ContentRequest : IDownloadContentInput
{
    [Display("Content type"), StaticDataSource(typeof(ContentTypeDataSourceHandler))]
    public string ContentType { get; set; } = string.Empty;

    [Display("Content ID"), DataSource(typeof(ContentDataSourceHandler))]
    public string ContentId { get; set; } = string.Empty;
}   
