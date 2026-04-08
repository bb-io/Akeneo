using Apps.Akeneo.Constants;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Akeneo.Models.Response.Content;

public record GetContentResponse : IDownloadContentInput
{
    [Display("Content ID")]
    public string ContentId { get; set; }

    [Display("Content name")]
    public string? ContentName { get; set; }

    [Display("Content type")]
    public string ContentType { get; set; }

    [Display("Family")]
    public string? Family { get; set; }

    [Display("Created at")]
    public DateTime Created { get; set; }

    [Display("Updated at")]
    public DateTime? Updated { get; set; }

    public GetContentResponse(ProductContentEntity productEntity, string locale)
    {
        ContentId = productEntity.Id;
        ContentName = productEntity.Values["name"].FirstOrDefault(x => x.Locale == locale)?.Data.ToString();
        ContentType = ContentTypeConstants.Product;
        Family = productEntity.Family;
        Created = productEntity.Created;
        Updated = productEntity.Updated;
    }

    public GetContentResponse(ProductModelEntity productModelEntity)
    {
        ContentId = productModelEntity.Id;
        ContentName = productModelEntity.Id;
        ContentType = ContentTypeConstants.ProductModel;
        Family = productModelEntity.Family;
        Created = productModelEntity.Created;
        Updated = productModelEntity.Updated;
    }
}
