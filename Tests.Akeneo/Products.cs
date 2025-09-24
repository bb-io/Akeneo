using Apps.Akeneo.Actions;
using Apps.Akeneo.DataSource;
using Apps.Akeneo.Models;
using Apps.Akeneo.Models.Request;
using Apps.Akeneo.Models.Request.Product;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Akeneo.Base;

namespace Tests.Akeneo;

[TestClass]
public class Products : TestBase
{
    public const string PRODUCT_ID = "005f730c-2e31-49a0-8172-96dc65fd9b20";
    public const string LOCALE = "en_US";

    [TestMethod]
    public async Task Get_product_as_html_works()
    {
        var actions = new ProductActions(InvocationContext, FileManager);

        var result = await actions.GetProductHtml(new ProductRequest { ProductId = PRODUCT_ID }, new LocaleRequest { Locale = LOCALE }, new OptionalFileTypeHandler { });

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
        Console.WriteLine(json);

        Assert.IsTrue(result.File != null);
    }

    [TestMethod]
    public async Task Update_product_from_html_works()
    {
        var actions = new ProductActions(InvocationContext, FileManager);

        var fileReference = new FileReference() { Name = "005f730c-2e31-49a0-8172-96dc65fd9b20.html" };
        await actions.UpdateProductHtml(new ProductOptionalRequest { }, new LocaleRequest { Locale = LOCALE }, new FileModel { File = fileReference });
    }

    [TestMethod]
    public async Task Get_product_as_json_works()
    {
        var actions = new ProductActions(InvocationContext, FileManager);

        var result = await actions.GetProductHtml(new ProductRequest { ProductId = PRODUCT_ID }, new LocaleRequest { Locale = LOCALE }, new OptionalFileTypeHandler { FileType = "json" });

        Assert.IsTrue(result.File != null);
    }

    [TestMethod]
    public async Task Update_product_from_json_works()
    {
        var actions = new ProductActions(InvocationContext, FileManager);

        var fileReference = new FileReference() { Name = "005f730c-2e31-49a0-8172-96dc65fd9b20.json" };
        await actions.UpdateProductHtml(new ProductOptionalRequest { }, new LocaleRequest { Locale = LOCALE }, new FileModel { File = fileReference });
    }
}