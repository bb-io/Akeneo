using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Akeneo.Models.Entities
{
    public class ProductModelEntity
    {
        [Display("Product model code"), JsonProperty("code")]
        public string Id { get; set; }

        public string Family { get; set; }

        public string? Parent { get; set; }

        public IEnumerable<string> Categories { get; set; }

        [Display("Created at")]
        public DateTime Created { get; set; }

        [Display("Updated at")]
        public DateTime? Updated { get; set; }
    }
}
