using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Response;

public record LabelResponse(string Locale, string Value)
{
    [Display("Label locale")]
    public string Locale { get; set; } = Locale;

    [Display("Label value")]
    public string Value { get; set; } = Value;
}
