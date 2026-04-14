using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Polling.Models.Request;

public class ContentFilter
{
    [Display("Name contains")]
    public string? NameContains { get; set; }
}
