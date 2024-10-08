using Newtonsoft.Json;

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
