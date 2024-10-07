using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.ProductModel
{
    public class ProductModelRequest
    {
        [Display("Product model code")]
        [DataSource(typeof(ProductModelDataSourceHandler))]
        public string ProductModelCode { get; set; }
    }
}
