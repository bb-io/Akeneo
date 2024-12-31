using Apps.Akeneo.Invocables;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Akeneo.Actions;

[ActionList]
public class DebugActions(InvocationContext invocationContext) : AkeneoInvocable(invocationContext)
{
    [Action("[DEBUG] Action", Description = "Debug action")]
    public List<AuthenticationCredentialsProvider> DebugAction()
    {
        return Creds.ToList();
    }
}