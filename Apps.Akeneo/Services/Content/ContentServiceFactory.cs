using Apps.Akeneo.Constants;
using Apps.Akeneo.Services.Content.Concrete;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.Akeneo.Services.Content;

public class ContentServiceFactory(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
{
    public IContentService GetContentService(string contentType)
    {
        return contentType switch
        {
            ContentTypeConstants.Product => new ProductContentService(invocationContext, fileManagementClient),
            ContentTypeConstants.ProductModel => new ProductModelContentService(invocationContext, fileManagementClient),
            _ => throw new Exception($"Unsupported content type '{contentType}' was passed to ContentServiceFactory")
        };
    }

    public List<IContentService> GetContentServices(IEnumerable<string> contentTypes)
    {
        var contentServices = new List<IContentService>();

        foreach (var contentType in contentTypes)
            contentServices.Add(GetContentService(contentType));

        return contentServices;
    }
}
