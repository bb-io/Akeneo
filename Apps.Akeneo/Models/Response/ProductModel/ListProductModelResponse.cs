using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Models.Response.ProductModel
{
    public class ListProductModelResponse
    {
        [Display("Product models")]
        public IEnumerable<ProductModelEntity> ProductModels { get; set; }
    }
}
