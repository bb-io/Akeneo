using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Queries
{
    public class SearchQuery
    {
        private const string AkeneoDateFormat = "yyyy-MM-dd HH:mm:ss";
        private readonly Dictionary<string, List<QueryOperator>> _queries = [];

        public void Add(string key, QueryOperator op)
        {
            if (op.Value != null)
            {
                EnsureKeyExists(key);
                _queries[key].Add(op);
            }
        }

        public void AddDateAfter(string key, DateTime? after)
        {
            if (after.HasValue)
            {
                EnsureKeyExists(key);
                _queries[key].Add(new QueryOperator { Operator = ">", Value = after.Value.ToString(AkeneoDateFormat) });
            }
        }

        public void AddDateBefore(string key, DateTime? before)
        {
            if (before.HasValue)
            {
                EnsureKeyExists(key);
                _queries[key].Add(new QueryOperator { Operator = "<", Value = before.Value.ToString(AkeneoDateFormat) });
            }
        }

        private void EnsureKeyExists(string key)
        {
            if (!_queries.ContainsKey(key))
                _queries[key] = [];
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(_queries, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
        }
    }
}
