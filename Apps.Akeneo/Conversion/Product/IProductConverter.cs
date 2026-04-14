using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Akeneo.Conversion.Product;

public interface IProductConverter
{
    Task<FileReference> ToOutputFile(IContentEntity productContent, string locale, string? scope, bool ignoreNonScopable);
}
