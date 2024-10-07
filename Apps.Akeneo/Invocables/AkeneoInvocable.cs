using Apps.Akeneo.Api;
using Apps.Akeneo.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Akeneo.Invocables;

public class AkeneoInvocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    protected AkeneoClient Client { get; }
    protected string ClientId { get; }
    protected string ClientSecret { get; }
    protected string Url { get; set; }

    public AkeneoInvocable(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new(Creds);
        ClientId = Creds.Get(CredsNames.ClientID).Value;
        ClientSecret = Creds.Get(CredsNames.ClientSecret).Value;
        Url = Creds.Get(CredsNames.Url).Value;
    }
}