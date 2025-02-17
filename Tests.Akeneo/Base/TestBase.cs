using Apps.Akeneo.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Akeneo.Base;
public class TestBase
{
    public IEnumerable<AuthenticationCredentialsProvider> Creds { get; set; }

    public InvocationContext InvocationContext { get; set; }

    public FileManager FileManager { get; set; }

    public TestBase()
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var _creds = config.GetSection("ConnectionDefinition").GetChildren()
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();

        Creds = GetAccessToken(_creds);

        var relativePath = config.GetSection("TestFolder").Value;
        var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        var folderLocation = Path.Combine(projectDirectory, relativePath);

        InvocationContext = new InvocationContext
        {
            AuthenticationCredentialsProviders = Creds,
        };

        FileManager = new FileManager();
    }

    public IEnumerable<AuthenticationCredentialsProvider> GetAccessToken(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var url = creds.Get(CredsNames.Url).Value;
        var clientId = creds.Get(CredsNames.ClientID).Value;
        var clientSecret = creds.Get(CredsNames.ClientSecret).Value;
        var username = creds.Get("username").Value;
        var password = creds.Get("password").Value;

        var options = new RestClientOptions(url)
        {
            Authenticator = new HttpBasicAuthenticator(clientId, clientSecret)
        };

        var client = new RestClient(options);

        var request = new RestRequest("/api/oauth/v1/token", Method.Post);
        request.AddJsonBody(new { grant_type = "password", username, password});
        var response = client.Execute<TokenResponse>(request);
        return creds.Append(new AuthenticationCredentialsProvider("access_token", response.Data.access_token ));
    }
}

public class TokenResponse
{
    public string access_token { get; set; }
}
