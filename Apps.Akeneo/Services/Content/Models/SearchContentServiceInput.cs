using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Content;

namespace Apps.Akeneo.Services.Content.Models;

public class SearchContentServiceInput(SearchContentRequest searchContentRequest, LocaleRequest localeRequest)
{
    public string Locale { get; set; } = localeRequest.Locale;
    public bool? IsEnabled { get; set; } = searchContentRequest.IsEnabled ?? true;
    public DateTime? UpdatedAfter { get; set; } = searchContentRequest.UpdatedAfter;
    public DateTime? UpdatedBefore { get; set; } = searchContentRequest.UpdatedBefore;
    public DateTime? CreatedAfter { get; set; } = searchContentRequest.CreatedAfter;
    public DateTime? CreatedBefore { get; set; } = searchContentRequest.CreatedBefore;
}
