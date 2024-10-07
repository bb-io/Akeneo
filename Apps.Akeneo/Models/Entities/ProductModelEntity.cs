using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Entities
{
    public class ProductModelEntity
    {
        [Display("Product model code")]
        public string Code { get; set; }

        public string Family { get; set; }

        public string? Parent { get; set; }

        public IEnumerable<string> Categories { get; set; }

        [Display("Created at")]
        public DateTime Created { get; set; }

        [Display("Updated at")]
        public DateTime? Updated { get; set; }
    }
}
