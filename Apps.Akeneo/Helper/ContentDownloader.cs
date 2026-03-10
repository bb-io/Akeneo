using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Xliff.Xliff2;
using HtmlAgilityPack;
using System.Text;

namespace Apps.Akeneo.Helper;

public static class ContentDownloader
{
    public async static Task<HtmlDocument> GetHtmlFromFile(Stream fileStream)
    {
        var html = Encoding.UTF8.GetString(await fileStream.GetByteData());

        if (Xliff2Serializer.IsXliff2(html))
        {
            html = Transformation.Parse(html, $"file.xlf").Target().Serialize() ??
                throw new PluginMisconfigurationException("XLIFF did not contain files");
        }

        var doc = new HtmlDocument();
        doc.Load(html);

        return doc;
    }
}
