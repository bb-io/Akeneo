using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request;

public class CatalogRequest
{
    [Display("Catalog ID"), DataSource(typeof(CatalogDataSourceHandler))]
    public string CatalogId { get; set; }
}