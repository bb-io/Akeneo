using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response;

public class LabelResponse
{
    [Display("Label locale")]
    public string Locale { get; set; } = string.Empty;

    [Display("Label value")]
    public string Value { get; set; } = string.Empty;
}
