using System.Diagnostics;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.CloudPlayground.Tests;

public class CheckMessageBusStatusConsumer : IConsumer<CheckMessageBusStatus>
{
    public async Task Consume(ConsumeContext<CheckMessageBusStatus> context)
    {
        await context.RespondAsync<MessageBusStatusResult>(new
        {
            IsHealthy = true,
            TimeStamp = DateTime.Now
        });
    }
}