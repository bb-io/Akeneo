using Apps.Akeneo.Constants;
using Apps.Akeneo.Extensions;
using Apps.Akeneo.HtmlConversion;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Utility;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json.Linq;
using System.Net.Mime;

namespace Apps.Akeneo.Helper;

public static class ContentTypeDetector
{
    public static async Task<DetectedContent> DetectFromFile(Stream fileStream, FileReference initialFile)
    {
        if (initialFile.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            using var reader = new StreamReader(fileStream, leaveOpen: true);
            var json = await reader.ReadToEndAsync();
            fileStream.Position = 0;

            var jObject = JObject.Parse(json);
            string? contentType = jObject["content_type"]?.ToString();
            if (!string.IsNullOrWhiteSpace(contentType))
                return new(contentType, MediaTypeNames.Application.Json, json);

            throw new PluginMisconfigurationException(
                @"Unknown content type - can't detect automatically from JSON structure. 
                Please specify the content type manually.");
        }
        else
        {
            using var reader = new StreamReader(fileStream, leaveOpen: true);
            string html = await reader.ReadToEndAsync();
            fileStream.Position = 0;
            var doc = await ContentDownloader.LoadHtmlDocument(html);

            string? contentType = doc.ExtractMetadata(HtmlConstants.ContentType);
            if (!string.IsNullOrWhiteSpace(contentType))
                return new(contentType, MediaTypeNames.Text.Html, doc);

            throw new PluginMisconfigurationException(
                @"Unknown content type - can't detect automatically from HTML/XLIFF metadata. 
                Please specify the content type manually.");
        }
    }

    public static string DetectFromType(IContentEntity contentEntity)
    {
        return contentEntity switch
        {
            ProductContentEntity => ContentTypeConstants.Product,
            ProductModelContentEntity => ContentTypeConstants.ProductModel,
            _ => throw new PluginMisconfigurationException($"Cannot infer content type for {contentEntity.GetType().Name}")
        };
    }
}
