using Apps.Akeneo.Constants;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Akeneo.DataSource.Static;

public class ContentTypeDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new DataSourceItem(ContentTypeConstants.Product, "Product"),
            new DataSourceItem(ContentTypeConstants.ProductModel, "Product model")
        ];
    }
}
