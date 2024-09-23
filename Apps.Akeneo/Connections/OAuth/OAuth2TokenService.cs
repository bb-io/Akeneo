using System.Security.Cryptography;
using System.Text;
using Apps.Akeneo.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using RestSharp;

namespace Apps.Akeneo.Connections.OAuth;

public class OAuth2TokenService : BaseInvocable, IOAuth2TokenService
{
    public OAuth2TokenService(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> RequestToken(string state, string code,
        Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        var instanceUrl = InvocationContext.AuthenticationCredentialsProviders.Get(CredsNames.Url).Value.TrimEnd('/');
        var endpoint = $"{instanceUrl}/connect/apps/v1/oauth2/token";

        var formParameters = new Dictionary<string, string>()
        {
            ["client_id"] = ApplicationConstants.ClientId,
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["code_identifier"] = state,
            ["code_challenge"] = GetCodeChallenge(state),
        };
        var request = new RestRequest(endpoint, Method.Post);
        formParameters.ToList().ForEach(x => request.AddParameter(x.Key, x.Value));

        var response = await new RestClient().ExecuteAsync<Dictionary<string, string>>(request, cancellationToken);
        return response.Data!;
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    public bool IsRefreshToken(Dictionary<string, string> values) => false;

    private string GetCodeChallenge(string codeIdentifier)
    {
        var dataToHash = codeIdentifier +  ApplicationConstants.ClientSecret;

        var byteArray = Encoding.UTF8.GetBytes(dataToHash);

        var hashBytes = SHA256.Create().ComputeHash(byteArray);

        var hashHex = new StringBuilder();
        foreach (var b in hashBytes)
            hashHex.Append(b.ToString("x2"));

        return hashHex.ToString();
    }
}