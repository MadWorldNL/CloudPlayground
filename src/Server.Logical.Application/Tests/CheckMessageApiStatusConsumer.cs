using MassTransit;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.CloudPlayground.Tests;

public class CheckMessageApiStatusConsumer : IConsumer<CheckMessageApiStatus>
{
    private readonly ILogger<CheckMessageApiStatusConsumer> _logger;

    public CheckMessageApiStatusConsumer(ILogger<CheckMessageApiStatusConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<CheckMessageApiStatus> context)
    {
        _logger.LogInformation("Api Status send message: {Message}", context.Message.Message);
        
        return Task.CompletedTask;
    }
}