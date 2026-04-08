using Apps.Akeneo.Constants;
using Apps.Akeneo.DataSource.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Akeneo.Models.Request.Content;

public class ContentTypesRequest
{
    [Display("Content types", Description = "If not specified, all content types are applied")]
    [StaticDataSource(typeof(ContentTypeDataSourceHandler))]
    public IEnumerable<string>? ContentTypes { get; set; }

    public ContentTypesRequest ApplyDefaultValues()
    {
        if (ContentTypes == null || !ContentTypes.Any())
            ContentTypes = ContentTypeConstants.SupportedContentTypes;

        return this;
    }
}
