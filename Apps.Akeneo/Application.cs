using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.Akeneo;

public class Application : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.ECommerce];
        set { }
    }

    public string Name
    {
        get => "Akeneo";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}