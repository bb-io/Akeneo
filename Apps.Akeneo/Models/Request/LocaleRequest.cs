using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Akeneo.Models.Request;

public class LocaleRequest
{
    [Display("Locale"), DataSource(typeof(LocaleDataSourceHandler))]
    public string Locale { get; set; } = string.Empty;
}