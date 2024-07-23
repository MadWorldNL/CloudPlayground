using MassTransit;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.CloudPlayground.Tests;

public class CheckMessageBusStatusConsumer : IConsumer<CheckMessageBusStatus>
{
    private readonly ILogger<CheckMessageBusStatusConsumer> _logger;

    public CheckMessageBusStatusConsumer(ILogger<CheckMessageBusStatusConsumer> logger)
    {
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<CheckMessageBusStatus> context)
    {
        _logger.LogInformation("Api is healthy!");
        
        await context.RespondAsync<MessageBusStatusResult>(new
        {
            IsHealthy = true,
            TimeStamp = DateTime.Now
        });
    }
}