using System.Text;
using System.Web;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Exceptions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Akeneo.HtmlConversion;

public static class ProductHtmlConverter
{
    private const string ValueNameAttribute = "name";
    private const string ValueScopeAttribute = "scope";
    private const string ValueTypeAttribute = "type";
    private const string ResourceIdAttribute = "resource_id";

    private const string ArrayType = "array";
    private const string TableType = "table";

    public static Stream ToHtml(IContentEntity product, string locale, string? scope, bool ignoreNonScopable)
    {
        var (doc, body) = PrepareEmptyHtmlDocument();

        var filteredValues = product.Values
            .Where(kvp =>
            {
                if (!ignoreNonScopable) 
                    return true;

                bool isScopable = kvp.Value.Any(x => x.Scope != null);
                return isScopable;
            })
            .Select(kvp =>
                new KeyValuePair<string, ProductValueEntity[]>(
                    kvp.Key,
                    kvp.Value
                        .Where(val => val.Locale == locale)
                        .Where(val => scope is null || val.Scope == scope || val.Scope == null)
                        .ToArray()
                ))
            .Where(kvp => kvp.Value.Length > 0);

        foreach (var item in filteredValues)
            ConvertProductValue(item, doc, body);

        doc.DocumentNode.FirstChild.SetAttributeValue(ResourceIdAttribute, product.Id);

        var htmlBytes = Encoding.UTF8.GetBytes(doc.DocumentNode.OuterHtml);
        return new MemoryStream(htmlBytes);
    }

    public static T UpdateFromHtml<T>(T product, string locale, HtmlDocument doc, string? channel) where T : IContentEntity
    {
        var valueNodes = doc.DocumentNode.SelectSingleNode("//body").ChildNodes
            .Where(x => x.Attributes[ValueNameAttribute]?.Value is not null)
            .ToArray();

        foreach (var valueNode in valueNodes)
        {
            var attributeName = valueNode.Attributes[ValueNameAttribute].Value; 
            var htmlScope = valueNode.Attributes[ValueScopeAttribute]?.Value;
            var nodeScope = channel ?? htmlScope;

            if (nodeScope != null && product.Values.TryGetValue(attributeName, out var existingValues) && 
                existingValues.Length > 0)
            {
                var isScopable = existingValues.Any(x => x.Scope != null);
                if (!isScopable)
                {
                    throw new PluginMisconfigurationException(
                        $"The attribute '{attributeName}' is not scopable - it cannot be updated for a specific channel. " +
                        $"To upload content without these attributes, " +
                        $"set the 'Ignore global non-scopable attributes' input to true when downloading content");
                }
            }

            object nodeData = valueNode.Attributes[ValueTypeAttribute]?.Value switch
            {
                ArrayType => GetArrayFromHtml(valueNode),
                TableType => GetTableFromHtml(valueNode),
                _ => valueNode.InnerHtml.Trim().Trim('\"')
            };

            if (!product.Values.ContainsKey(attributeName))
                product.Values[attributeName] = [];

            var valuesList = product.Values[attributeName];
            var valueEntity = valuesList.FirstOrDefault(x => x.Locale == locale && x.Scope == nodeScope);

            if (valueEntity != null)
                valueEntity.Data = nodeData;
            else
            {
                var newValue = new ProductValueEntity
                {
                    Locale = locale,
                    Scope = nodeScope,
                    Data = nodeData
                };

                product.Values[attributeName] = valuesList.Append(newValue).ToArray();
            }
        }

        return product;
    }
    
    public static string GetResourceId(HtmlDocument doc)
    {
        return doc.DocumentNode.FirstChild.Attributes[ResourceIdAttribute]?.Value ?? string.Empty;
    }

    private static JArray GetArrayFromHtml(HtmlNode valueNode)
    {
        var arrayContent = valueNode
            .Descendants()
            .Where(x => x.Name == HtmlConstants.Li)
            .Select(x => HttpUtility.HtmlDecode(x.InnerText));

        return JArray.FromObject(arrayContent);
    }

    private static JArray GetTableFromHtml(HtmlNode valueNode)
    {
        var headers = valueNode
            .Descendants()
            .Where(x => x.Name == HtmlConstants.Th)
            .Select(x => HttpUtility.HtmlDecode(x.InnerText))
            .ToArray();

        var result = new JArray();

        var rows = valueNode.SelectNodes(".//tr[td]");
        foreach (var row in rows)
        {
            var obj = new JObject();
            var cells = row.SelectNodes(HtmlConstants.Td);

            for (var i = 0; i < cells.Count; i++)
                obj[headers[i]] = HttpUtility.HtmlDecode(cells[i].InnerText.Trim());

            result.Add(obj);
        }

        return result;
    }

    private static void ConvertProductValue(KeyValuePair<string, ProductValueEntity[]> value, HtmlDocument doc,
        HtmlNode body)
    {
        foreach (var productValueEntity in value.Value)
        {
            var valueNode = doc.CreateElement(HtmlConstants.Div);

            if (productValueEntity.Data is JArray arr)
                ConvertArrayToHtml(arr, doc, valueNode);
            else
                valueNode.InnerHtml = ConvertContentToHtml(productValueEntity.Data);

            valueNode.SetAttributeValue(ValueNameAttribute, value.Key);
            valueNode.SetAttributeValue(ValueScopeAttribute, productValueEntity.Scope);

            body.AppendChild(valueNode);
        }
    }

    private static void ConvertArrayToHtml(JArray array, HtmlDocument doc, HtmlNode valueNode)
    {
        if (array.First() is JObject)
        {
            ConvertTableToHtml(array, doc, valueNode);
            return;
        }

        var ulNode = doc.CreateElement(HtmlConstants.Ul);

        foreach (var item in array)
        {
            var liNode = doc.CreateElement(HtmlConstants.Li);
            liNode.InnerHtml = item.ToString();

            ulNode.AppendChild(liNode);
        }

        valueNode.SetAttributeValue(ValueTypeAttribute, ArrayType);
        valueNode.AppendChild(ulNode);
    }

    private static void ConvertTableToHtml(JArray array, HtmlDocument doc, HtmlNode valueNode)
    {
        var jObjects = array.OfType<JObject>().ToArray();
        var tableNode = doc.CreateElement(HtmlConstants.Table);

        var headerRow = doc.CreateElement(HtmlConstants.Tr);
        var tableColumns = jObjects.MaxBy(x => x.Properties()!.Count()).Properties().ToArray();

        foreach (var property in tableColumns)
        {
            var headerCell = doc.CreateElement(HtmlConstants.Th);
            headerCell.InnerHtml = property.Name;
            headerRow.AppendChild(headerCell);
        }

        tableNode.AppendChild(headerRow);

        foreach (var obj in jObjects)
        {
            var row = doc.CreateElement(HtmlConstants.Tr);
            foreach (var tableColumn in tableColumns)
            {
                var cell = doc.CreateElement(HtmlConstants.Td);
                cell.InnerHtml = obj[tableColumn.Name]?.ToString() ?? string.Empty;
                row.AppendChild(cell);
            }

            tableNode.AppendChild(row);
        }

        valueNode.SetAttributeValue(ValueTypeAttribute, TableType);
        valueNode.AppendChild(tableNode);
    }

    private static string ConvertContentToHtml(object obj) =>
        "\"" + (obj as string ?? JsonConvert.SerializeObject(obj)) + "\"";

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
}