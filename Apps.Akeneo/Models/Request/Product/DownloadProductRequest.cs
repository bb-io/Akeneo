using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Request.Product;

public class DownloadProductRequest
{
    [Display("Ignore global non-scopable attributes",
        Description =
        "If downloading a specific channel, choose whether to also include attributes " +
        "that don't have a channel (global attributes). Default is false.")]
    public bool? IgnoreNonScopable { get; set; }
}
