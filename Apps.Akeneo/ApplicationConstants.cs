namespace Apps.Akeneo;

public static class ApplicationConstants
{
    //public const string ClientId = "#{AKENEO_CLIENT_ID}#";
    //public const string ClientSecret = "#{AKENEO_SECRET}#";
    public const string Scopes =
        "read_products write_products delete_products " +
        "read_locales read_channel_localization read_catalog_structure write_catalog_structure " +
        "read_attribute_options write_attribute_options " +
        "read_categories write_categories " +
        "read_channel_settings";
}