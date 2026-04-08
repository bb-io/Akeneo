using HtmlAgilityPack;

namespace Apps.Akeneo.Extensions;

public static class HtmlDocumentExtensions
{
    public static HtmlDocument InjectMetadata(this HtmlDocument doc, string metaName, string? metaValue)
    {
        if (string.IsNullOrWhiteSpace(metaValue) || string.IsNullOrWhiteSpace(metaName))
            return doc;

        var head = doc.DocumentNode.SelectSingleNode("//head");
        if (head == null)
        {
            head = doc.CreateElement("head");
            var htmlNode = doc.DocumentNode.SelectSingleNode("//html") ?? doc.DocumentNode;
            htmlNode.PrependChild(head);
        }

        var metaTag = doc.CreateElement("meta");
        metaTag.SetAttributeValue("name", metaName);
        metaTag.SetAttributeValue("content", metaValue);
        head.AppendChild(metaTag);
        return doc;
    }
}
