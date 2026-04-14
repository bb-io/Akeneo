using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System.Net.Mime;

namespace Apps.Akeneo.DataSource.Static;

public class FileTypeDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new DataSourceItem(MediaTypeNames.Text.Html, "Interoperable HTML (default)"),
            new DataSourceItem(MediaTypeNames.Application.Json, "Original JSON")    
        ];
    }
}
