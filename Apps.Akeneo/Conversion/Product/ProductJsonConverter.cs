using Apps.Akeneo.Extensions;
using Apps.Akeneo.Helper;
using Apps.Akeneo.Models.Entities;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Text;

namespace Apps.Akeneo.Conversion.Product;

public class ProductJsonConverter : IProductConverter
{
    public async Task<FileReference> ToOutputFile(
        IContentEntity productContent,
        string locale, 
        string? scope, 
        bool ignoreNonScopable,
        IFileManagementClient fileManagementClient)
    {
        if (productContent.Values != null)
        {
            productContent.Values = productContent.Values
                .Where(kvp =>
                {
                    if (!ignoreNonScopable)
                        return true;

                    return kvp.Value.Any(x => x.Scope != null);
                })
                .Select(kvp => new KeyValuePair<string, ProductValueEntity[]>(
                    kvp.Key,
                    kvp.Value
                        .Where(val => val.Locale == null || val.Locale == locale)
                        .Where(val => scope == null || val.Scope == null || val.Scope == scope)
                        .ToArray()
                ))
                .Where(kvp => kvp.Value.Length > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        var jObject = JObject.FromObject(productContent);

        string contentType = ContentTypeDetector.DetectFromType(productContent);
        jObject.AddFirst(new JProperty("content_type", contentType));

        var jsonBytes = Encoding.UTF8.GetBytes(jObject.ToString(Formatting.Indented));
        var jsonStream = new MemoryStream(jsonBytes);
        string jsonFileName = productContent.Id.ToFileName("json");
        return await fileManagementClient.UploadAsync(jsonStream, MediaTypeNames.Application.Json, jsonFileName);
    }

    public IContentEntity UpdateFromFile(object inputFile, string? contentId, string locale, string? scope)
    {
        string jsonContent = inputFile as string ?? 
            throw new PluginMisconfigurationException("Could not convert JSON payload to string");

        var updatedProduct = JsonConvert.DeserializeObject<ProductContentEntity>(jsonContent) ??
            throw new PluginMisconfigurationException("Could not deserialize input JSON payload");

        foreach (var kvp in updatedProduct.Values)
        {
            foreach (var value in kvp.Value)
            {
                if (value.Locale != null && !string.IsNullOrWhiteSpace(locale))
                    value.Locale = locale;

                if (value.Scope != null && !string.IsNullOrWhiteSpace(scope))
                    value.Scope = scope;
            }

            updatedProduct.Values[kvp.Key] = kvp.Value
                .GroupBy(x => new { x.Locale, x.Scope })
                .Select(g => g.Last())
                .ToArray();
        }

        updatedProduct.Id = contentId ?? updatedProduct.Id;
        return updatedProduct;
    }
}
