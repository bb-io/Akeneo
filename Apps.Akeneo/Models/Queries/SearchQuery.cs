using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Queries
{
    public class SearchQuery
    {
        private Dictionary<string, IEnumerable<QueryOperator>> _queries;
        public SearchQuery() 
        {
            _queries = new Dictionary<string, IEnumerable<QueryOperator>>();
        }

        public void Add(string key, QueryOperator op) 
        {
            if (op.Value != null)
            {
                _queries.Add(key, new List<QueryOperator> { op });
            }
        }

        public string ToString()
        {
            return JsonConvert.SerializeObject(_queries, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
        }
    }
}
