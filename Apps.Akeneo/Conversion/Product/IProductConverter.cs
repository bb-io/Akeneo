using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Akeneo.Conversion.Product;

public interface IProductConverter
{
    Task<FileReference> ToOutputFile(
        IContentEntity productContent, 
        string locale, 
        string? scope, 
        bool ignoreNonScopable,
        IFileManagementClient fileManagementClient);
    IContentEntity UpdateFromFile(object inputFile, string? contentId, string locale, string? scope);
}
