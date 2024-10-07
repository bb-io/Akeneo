using System.Text;
using Apps.Akeneo.Models.Entities;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Apps.Akeneo.HtmlConversion;

public static class ProductHtmlConverter
{
    private const string ValueNameAttribute = "name";
    private const string ValueScopeAttribute = "scope";

    public static Stream ToHtml(IContentEntity product, string locale)
    {
        var (doc, body) = PrepareEmptyHtmlDocument();
        product.Values
            .Select(x =>
                new KeyValuePair<string, ProductValueEntity[]>(x.Key, x.Value.Where(x => x.Locale == locale).ToArray()))
            .Where(x => x.Value != null)
            .ToList()
            .ForEach(x => ConvertProductValue(x, doc, body));

        var htmlBytes = Encoding.UTF8.GetBytes(doc.DocumentNode.OuterHtml);
        return new MemoryStream(htmlBytes);
    }

    private static void ConvertProductValue(KeyValuePair<string, ProductValueEntity[]> value, HtmlDocument doc,
        HtmlNode body)
    {
        foreach (var productValueEntity in value.Value)
        {
            var valueNode = doc.CreateElement(HtmlConstants.Div);
            valueNode.InnerHtml =
                "\"" + (productValueEntity.Data as string ?? JsonConvert.SerializeObject(productValueEntity.Data)) + // TODO: No JSON in HTML
                "\"";

            valueNode.SetAttributeValue(ValueNameAttribute, value.Key);
            valueNode.SetAttributeValue(ValueScopeAttribute, productValueEntity.Scope);

            body.AppendChild(valueNode);
        }
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

    public static T UpdateFromHtml<T>(T product, string locale, Stream fileStream) where T: IContentEntity
    {
        var doc = new HtmlDocument();
        doc.Load(fileStream);

        var valueNodes = doc.DocumentNode.SelectSingleNode("//body").ChildNodes
            .Where(x => x.Attributes[ValueNameAttribute]?.Value is not null)
            .ToArray();

        foreach (var valueNode in valueNodes)
        {
            if (!product.Values.TryGetValue(valueNode.Attributes[ValueNameAttribute].Value, out var value))
                continue;

            var valueEntity = value.FirstOrDefault(x =>
                x.Locale == locale && x.Scope == valueNode.Attributes[ValueScopeAttribute]?.Value);

            if (valueEntity is null)
                continue;

            valueEntity.Data = valueNode.InnerHtml.Trim().Trim('\"');
        }

        return product;
    }
}