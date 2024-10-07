using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Models.Entities
{
    public interface IContentEntity
    {
        public Dictionary<string, ProductValueEntity[]> Values { get; set; }
    }
}
