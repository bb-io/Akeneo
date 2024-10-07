using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Models.Request.ProductModel
{
    public class SearchProductModelRequest
    {
        public string? Code { get; set; }

        [Display("Categories", Description = "Filter the result by products that are in all of the following categories")]
        [DataSource(typeof(CategoryDataSourceHandler))]
        public IEnumerable<string>? Categories { get; set; }

        [Display("Updated after")]
        public DateTime? Updated { get; set; }
    }
}
