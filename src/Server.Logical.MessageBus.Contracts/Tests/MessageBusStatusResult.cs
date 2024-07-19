namespace MadWorldNL.CloudPlayground.Tests;

public record MessageBusStatusResult()
{
    public bool IsHealthy { get; init; }
    public DateTime TimeStamp { get; init; }
}