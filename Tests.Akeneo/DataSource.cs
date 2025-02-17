using Apps.Akeneo.DataSource;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Akeneo.Base;

namespace Tests.Akeneo;

[TestClass]
public class DataSource : TestBase
{
    [TestMethod]
    public async Task Product_data_handler_works()
    {
        var handler = new ProductDataSourceHandler(InvocationContext);

        var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

        Console.WriteLine($"Total: {result.Count()}");
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value}: {item.DisplayName}");
        }

        Assert.IsTrue(result.Count() > 0);
    }
}
