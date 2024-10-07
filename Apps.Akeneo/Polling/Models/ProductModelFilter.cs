using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Polling.Models
{
    public class ProductModelFilter
    {
        [Display("Categories", Description = "Filter the result by products that contain any of the following categories")]
        [DataSource(typeof(CategoryDataSourceHandler))]
        public IEnumerable<string>? Categories { get; set; }
    }
}
