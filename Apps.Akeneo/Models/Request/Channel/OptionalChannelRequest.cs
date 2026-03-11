using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request.Channel;

public class OptionalChannelRequest
{
    [Display("Channel code"), DataSource(typeof(ChannelDataSourceHandler))]
    public string? ChannelCode { get; set; }
}
