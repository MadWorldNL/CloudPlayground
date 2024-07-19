using MadWorldNL.CloudPlayground.Tests;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.CloudPlayground.Endpoints;

public static class TestsEndpoints
{
    public static void AddTestsEndpoints(this WebApplication app)
    {
        var tests = app.MapGroup("/Tests");
        
        tests.MapGet("/CheckMessageBusStatus", async ([FromServices] IRequestClient<CheckMessageBusStatus> client, CancellationToken cancellationToken) =>
        {
            var response = await client.GetResponse<MessageBusStatusResult>(new CheckMessageBusStatus(), cancellationToken);
            return Results.Ok(response.Message);
        });
    }
}