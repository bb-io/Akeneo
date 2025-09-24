using System.Net.Mime;
using Apps.Akeneo.Constants;
using Apps.Akeneo.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Akeneo.Api;

public class AkeneoClient : BlackBirdRestClient
{
    protected override JsonSerializerSettings? JsonSettings => JsonConfig.Settings;

    public AkeneoClient(AuthenticationCredentialsProvider[] creds) : base(new()
    {
        BaseUrl = (creds.Get(CredsNames.Url).Value.TrimEnd('/') + "/api/rest/v1").ToUri()
    })
    {
        this.AddDefaultHeader("Accept", MediaTypeNames.Application.Json);
        this.AddDefaultHeader("Authorization", $"Bearer {creds.Get(CredsNames.AccessToken).Value}");
    }

    public async Task<List<T>> Paginate<T>(RestRequest request)
    {
        var baseUrl = request.Resource;
        var page = 1;
        var limit = 100;

        var result = new List<T>();
        PaginationResponse<T>? response;

        do
        {
            request.Resource = baseUrl.SetQueryParameter("page", page.ToString())
                .SetQueryParameter("limit", limit.ToString());

            response = await ExecuteWithErrorHandling<PaginationResponse<T>>(request);

            if (!string.IsNullOrEmpty(response.Error))
                throw new PluginApplicationException(response.Error);

            result.AddRange(response.Embedded.Items);

            page++;
        } while (response.Embedded.Items.Any());

        return result;
    }

    public async Task<IEnumerable<T>> PaginateOnce<T>(RestRequest request)
    {
        var baseUrl = request.Resource;
        var page = 1;
        var limit = 100;

        request.Resource = baseUrl.SetQueryParameter("page", page.ToString())
                .SetQueryParameter("limit", limit.ToString());

        var response = await ExecuteWithErrorHandling<PaginationResponse<T>>(request);

        if (!string.IsNullOrEmpty(response.Error))
            throw new PluginApplicationException(response.Error);

        return response.Embedded.Items;
    }

    public async Task<List<T>> PaginateUsingSearchAfter<T>(RestRequest request)
    {
        var result = new List<T>();
        PaginationResponse<T>? response;

        do
        {
            response = await ExecuteWithErrorHandling<PaginationResponse<T>>(request);

            if (!string.IsNullOrEmpty(response.Error))
                throw new PluginApplicationException(response.Error);

            result.AddRange(response.Embedded.Items);

            if (response.Links.Next?.Href != null)
                request.Resource = response.Links.Next?.Href;
        } while (response.Links.Next != null);

        return result;
    }

    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request)).Content;
        T val = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val;
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        RestResponse restResponse = await ExecuteAsync(request);
        if (!restResponse.IsSuccessStatusCode)
        {
            throw ConfigureErrorException(restResponse);
        }

        return restResponse;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (response.ErrorMessage != null)
        {
            return new PluginApplicationException(response.ErrorMessage);
        }

        var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);

        var errorMessage = error.Errors is null
            ? error.Message
            : $"{error.Message} {string.Join(" ", error.Errors.Select(x => x.Message))}";
        return new PluginApplicationException(errorMessage);
    }
}