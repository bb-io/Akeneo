using System.Net.Mime;
using System.Text;
using Apps.Akeneo.Constants;
using Apps.Akeneo.Models.Response;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Akeneo.Api;

public class AkeneoClient : BlackBirdRestClient
{
    private readonly AuthenticationCredentialsProvider[] _creds;
    private string? _accessToken;

    protected override JsonSerializerSettings? JsonSettings => JsonConfig.Settings;

    public AkeneoClient(AuthenticationCredentialsProvider[] creds) : base(new()
    {
        BaseUrl = (creds.Get(CredsNames.Url).Value.TrimEnd('/') + "/api/rest/v1").ToUri()
    })
    {
        _creds = creds;
        this.AddDefaultHeader("Accept", MediaTypeNames.Application.Json);
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            _accessToken = await GetToken();
            this.AddDefaultHeader("Authorization", $"Bearer {_accessToken}");
        }

        return await base.ExecuteWithErrorHandling(request);
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
            result.AddRange(response.Embedded.Items);

            page++;
        } while (response.Embedded.Items.Any());

        return result;
    }

    private async Task<string> GetToken()
    {
        var userName = _creds.Get(CredsNames.Username).Value;
        var password = _creds.Get(CredsNames.Password).Value;

        var endpoint = $"{_creds.Get(CredsNames.Url).Value.TrimEnd('/')}/api/oauth/v1/token";
        var request = new RestRequest(endpoint, Method.Post)
            .WithJsonBody(new
            {
                grant_type = "password",
                username = _creds.Get(CredsNames.Username).Value,
                password = _creds.Get(CredsNames.Password).Value,
            })
            .WithHeaders(new()
            {
                ["Authorization"] = $"Basic {Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(ApplicationConstants.ClientId + ":" + ApplicationConstants.ClientSecret))}"
            });

        var response = await ExecuteAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new($"Something went wrong during acquiring the token: {response.Content}");

        var authKeys = JsonConvert.DeserializeObject<AuthKeysResponse>(response.Content!, JsonSettings)!;
        return authKeys.AccessToken;
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
        return new(error.Message);
    }
}