using Apps.Akeneo.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.Akeneo.Helper;

public static class PollingHelper
{
    public static PollingEventResponse<DateMemory, T> NoFlight<T>()
    {
        return new()
        {
            FlyBird = false,
            Memory = new() { LastInteractionDate = DateTime.UtcNow }
        };
    }

    public static PollingEventResponse<DateMemory, T> TriggerFlight<T>(T result)
    {
        return new()
        {
            FlyBird = true,
            Result = result,
            Memory = new() { LastInteractionDate = DateTime.UtcNow }
        };
    }
}