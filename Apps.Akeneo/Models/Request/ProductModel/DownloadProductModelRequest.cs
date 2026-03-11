using Blackbird.Applications.Sdk.Common;

namespace Apps.Akeneo.Models.Request.ProductModel;

public class DownloadProductModelRequest
{
    [Display("Ignore global non-scopable attributes", 
        Description = "Whether to ignore attributes that are global and not updatable for specific channels. Default is false.")]
    public bool? IgnoreNonScopable { get; set; }
}
