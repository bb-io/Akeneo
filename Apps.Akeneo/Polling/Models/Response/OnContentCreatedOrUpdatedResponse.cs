using Apps.Akeneo.Models.Response.Content;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Akeneo.Polling.Models.Response;

public record OnContentCreatedOrUpdatedResponse(List<GetContentResponse> Items) 
    : IMultiDownloadableContentOutput<GetContentResponse>
{
    [Display("Items")]
    public List<GetContentResponse> Items { get; set; } = Items;
}
