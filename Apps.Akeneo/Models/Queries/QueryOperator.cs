using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Models.Queries
{
    public class QueryOperator
    {
        [JsonProperty("operator")]
        public string Operator {  get; set; }

        [JsonProperty("locale")]
        public string? Locale { get; set; }

        [JsonProperty("value")]
        public object? Value { get; set; }
    }
}
