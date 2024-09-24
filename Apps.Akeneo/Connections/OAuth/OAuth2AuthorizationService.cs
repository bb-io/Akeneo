using Apps.Akeneo.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.Akeneo.Connections.OAuth;

public class OAuth2AuthorizationService : BaseInvocable, IOAuth2AuthorizeService
{
    public OAuth2AuthorizationService(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        var bridgeOauthUrl = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/oauth";
        var instanceUrl = values[CredsNames.Url].TrimEnd('/');
        
        var parameters = new Dictionary<string, string>
        {
            { "scope", ApplicationConstants.Scopes },
            { "client_id", ApplicationConstants.ClientId },
            { "response_type", "code" },
            { "state", values["state"] },
            { "authorization_url", $"{instanceUrl}/connect/apps/v1/authorize" },
            { "actual_redirect_uri", InvocationContext.UriInfo.AuthorizationCodeRedirectUri.ToString() },
        };

        return QueryHelpers.AddQueryString(bridgeOauthUrl, parameters);
    }
}