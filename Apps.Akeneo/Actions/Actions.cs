using Apps.Akeneo.Invocables;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Akeneo.Actions;

[ActionList]
public class Actions : AppInvocable
{
    public Actions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
}