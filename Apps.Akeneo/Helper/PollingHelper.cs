using Apps.Akeneo.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.Akeneo.Helper;

public static class PollingHelper
{
    public static PollingEventResponse<HashMemory, T> NoFlight<T>(HashMemory? currentMemory)
    {
        currentMemory ??= new HashMemory();
        currentMemory.LastInteractionDate = DateTime.UtcNow;

        return new()
        {
            FlyBird = false,
            Memory = currentMemory
        };
    }

    public static PollingEventResponse<HashMemory, T> TriggerFlight<T>(T result, HashMemory currentMemory)
    {
        currentMemory.LastInteractionDate = DateTime.UtcNow;

        return new()
        {
            FlyBird = true,
            Result = result,
            Memory = currentMemory
        };
    }
}