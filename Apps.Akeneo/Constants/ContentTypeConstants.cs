namespace Apps.Akeneo.Constants;

public static class ContentTypeConstants
{
    public const string Product = "product";
    public const string ProductModel = "product-model";

    public static readonly IEnumerable<string> SupportedContentTypes = [ProductModel, Product];
}
