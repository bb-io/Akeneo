using System.Text;
using Apps.Akeneo.Models.Entities;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Akeneo.HtmlConversion;

public static class ProductHtmlConverter
{
    private const string ValueNameAttribute = "name";

    public static Stream ToHtml(ProductContentEntity product, string locale)
    {
        var (doc, body) = PrepareEmptyHtmlDocument();
        product.Values
            .Select(x =>
                new KeyValuePair<string, ProductValueEntity?>(x.Key, x.Value.FirstOrDefault(x => x.Locale == locale)))
            .Where(x => x.Value != null)
            .ToList()
            .ForEach(x => ConvertProductValue(x!, doc, body));

        var htmlBytes = Encoding.UTF8.GetBytes(doc.DocumentNode.OuterHtml);
        return new MemoryStream(htmlBytes);
    }

    private static void ConvertProductValue(KeyValuePair<string, ProductValueEntity> value, HtmlDocument doc,
        HtmlNode body)
    {
        var valueNode = doc.CreateElement(HtmlConstants.Div);
        valueNode.InnerHtml = JsonConvert.SerializeObject(value.Value.Data);
        valueNode.SetAttributeValue(ValueNameAttribute, value.Key);

        body.AppendChild(valueNode);
    }

    private static (HtmlDocument document, HtmlNode bodyNode) PrepareEmptyHtmlDocument()
    {
        var htmlDoc = new HtmlDocument();
        var htmlNode = htmlDoc.CreateElement(HtmlConstants.Html);
        htmlDoc.DocumentNode.AppendChild(htmlNode);

        var headNode = htmlDoc.CreateElement(HtmlConstants.Head);
        htmlNode.AppendChild(headNode);

        var bodyNode = htmlDoc.CreateElement(HtmlConstants.Body);
        htmlNode.AppendChild(bodyNode);

        return (htmlDoc, bodyNode);
    }

    public static ProductContentEntity UpdateFromHtml(ProductContentEntity product, string locale, Stream fileStream)
    {
        var doc = new HtmlDocument();
        doc.Load(fileStream);

        var valueNodes = doc.DocumentNode.ChildNodes
            .Where(x => x.Attributes[ValueNameAttribute]?.Value is not null)
            .ToArray();

        foreach (var valueNode in valueNodes)
        {
            if (!product.Values.TryGetValue(valueNode.Attributes[ValueNameAttribute].Value, out var value))
                continue;

            var valueEntity = value.FirstOrDefault(x => x.Locale == locale);
                
            if(valueEntity is null)
                continue;
            
            valueEntity.Data = JToken.Parse(valueNode.InnerHtml);
        }

        return product;
    }
}