using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Akeneo.Models.Request.Attribute;

public class UpdateAttributeRequest
{
    [Display("Attribute group"), DataSource(typeof(AttributeGroupDataSourceHandler))]
    public string? AttributeGroup { get; set; }

    [Display("Label locales", Description = "Label locales. Must match the order of 'Label values' input")]
    public List<string>? LabelLocales { get; set; }

    [Display("Label values", Description = "Label values for each locale")]
    public List<string>? LabelValues { get; set; }

    public UpdateAttributeRequest Validate()
    {
        if (LabelLocales?.Count != LabelValues?.Count)
            throw new PluginMisconfigurationException("The number of label locales must match the number of label values");

        return this;
    }
}
