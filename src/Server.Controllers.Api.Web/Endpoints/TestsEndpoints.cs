using System.Diagnostics;
using MadWorldNL.CloudPlayground.Diagnostics;
using MadWorldNL.CloudPlayground.Tests;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.CloudPlayground.Endpoints;

public static class TestsEndpoints
{
    public static void AddTestsEndpoints(this WebApplication app)
    {
        var tests = app.MapGroup("/Tests");
        
        tests.MapGet("/CheckMessageBusStatus", async ([FromServices] IRequestClient<CheckMessageBusStatus> client, [FromServices] DiagnosticsConfig diagnosticsConfig, CancellationToken cancellationToken) =>
        {
            using var activity = diagnosticsConfig.Source.StartActivity("Test Message Bus")!;
            activity.AddEvent(new ActivityEvent("Trigger check is message bus healthy"));
            
            var response = await client.GetResponse<MessageBusStatusResult>(new CheckMessageBusStatus(), cancellationToken);
            return Results.Ok(response.Message);
        }).WithOpenApi();

        tests.MapPost("/CheckMessageApiStatus", async ([FromBody] CheckMessageApiStatus status, [FromServices] ISendEndpointProvider sendEndpointProvider, CancellationToken cancellationToken) =>
        {
            await sendEndpointProvider.Send(status, cancellationToken);
            return Results.Ok();
        }).WithOpenApi();
        
        tests.MapPut("/CheckMultiMessageApiStatus", async ([FromBody] CheckMultiMessageApiStatus status, [FromServices] IPublishEndpoint publishEndpoint, CancellationToken cancellationToken) =>
        {
            await publishEndpoint.Publish(status, cancellationToken);
            return Results.Ok();
        }).WithOpenApi();
    }
}