using Apps.Akeneo.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Models.Request.ProductModel
{
    public class UpdateProductModelRequest
    {
        public Dictionary<string, ProductValueEntity[]> Values { get; set; }

        public UpdateProductModelRequest(ProductModelContentEntity productContentEntity)
        {
            Values = productContentEntity.Values;
        }
    }
}
