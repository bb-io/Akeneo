using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Xliff.Xliff2;
using HtmlAgilityPack;

namespace Apps.Akeneo.Helper;

public static class ContentDownloader
{
    public async static Task<HtmlDocument> LoadHtmlDocument(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
            throw new PluginMisconfigurationException("XLIFF/HTML file is empty");

        if (Xliff2Serializer.IsXliff2(html))
        {
            html = Transformation.Parse(html, $"file.xlf").Target().Serialize() ??
                throw new PluginMisconfigurationException("XLIFF did not contain files");
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        return doc;
    }
}
