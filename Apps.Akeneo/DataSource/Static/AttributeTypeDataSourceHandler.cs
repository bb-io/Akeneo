using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.DataSource.Static;

public class AttributeTypeDataSourceHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData()
    {
        return
        [
            new DataSourceItem("pim_catalog_identifier", "Identifier"),
            new DataSourceItem("pim_catalog_text", "Text"),
            new DataSourceItem("pim_catalog_textarea", "Text area"),
            new DataSourceItem("pim_catalog_simpleselect", "Simple select"),
            new DataSourceItem("pim_catalog_multiselect", "Multi select"),
            new DataSourceItem("pim_catalog_boolean", "Yes/No"),
            new DataSourceItem("pim_catalog_date", "Date"),
            new DataSourceItem("pim_catalog_number", "Number"),
            new DataSourceItem("pim_catalog_metric", "Measurement"),
            new DataSourceItem("pim_catalog_price_collection", "Price"),
            new DataSourceItem("pim_catalog_image", "Image"),
            new DataSourceItem("pim_catalog_file", "File"),
            new DataSourceItem("pim_catalog_asset_collection", "Asset collection (Enterprise Edition only)"),
            new DataSourceItem("akeneo_reference_entity", "Reference entity single link (Enterprise Edition only)"),
            new DataSourceItem("akeneo_reference_entity_collection", "Reference entity multiple links (Enterprise Edition only)"),
            new DataSourceItem("pim_reference_data_simpleselect", "Reference data simple select"),
            new DataSourceItem("pim_reference_data_multiselect", "Reference data multi select"),
            new DataSourceItem("pim_catalog_table", "Table (Growth and Enterprise editions only)"),
            new DataSourceItem("pim_catalog_product_link", "Product link"),
        ];
    }
}
