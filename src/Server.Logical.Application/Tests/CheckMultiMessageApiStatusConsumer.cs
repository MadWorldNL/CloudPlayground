using MassTransit;
using Microsoft.Extensions.Logging;

namespace MadWorldNL.CloudPlayground.Tests;

public class CheckMultiMessageApiStatusConsumer : IConsumer<CheckMultiMessageApiStatus>
{
    private readonly ILogger<CheckMultiMessageApiStatusConsumer> _logger;

    public CheckMultiMessageApiStatusConsumer(ILogger<CheckMultiMessageApiStatusConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<CheckMultiMessageApiStatus> context)
    {
        _logger.LogInformation("Api Status publish message: {Message}", context.Message.Message);
        
        return Task.CompletedTask;
    }
}