using RestSharp;

namespace Apps.Akeneo.Extensions;

public static class RestRequestExtensions
{
    public static RestRequest AddQueryParameterIfNotNull(this RestRequest request, string paramName, string? paramValue)
    {
        if (string.IsNullOrWhiteSpace(paramValue) || string.IsNullOrWhiteSpace(paramName))
            return request;

        request.AddQueryParameter(paramName, paramValue);
        return request;
    }
}