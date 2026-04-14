using Apps.Akeneo.DataSource.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Akeneo.Models.Request;

public class OptionalFileTypeHandler
{
    [Display("File format"), StaticDataSource(typeof(FileTypeDataSourceHandler))]
    public string? FileType { get; set; }
}
