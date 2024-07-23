using MadWorldNL.CloudPlayground.Diagnostics;
using MadWorldNL.CloudPlayground.Endpoints;
using MadWorldNL.CloudPlayground.MessageBus;
using MadWorldNL.CloudPlayground.Tests;
using MassTransit;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<CheckMultiMessageApiStatusConsumer>();
    
    x.AddConsumer<CheckMessageBusStatusConsumer>()
        .Endpoint(e => e.Name = nameof(CheckMessageBusStatus));
    
    x.AddRequestClient<CheckMessageBusStatus>(new Uri($"exchange:{nameof(CheckMessageBusStatus)}"));
    
    x.UsingRabbitMq((context,cfg) =>
    {
        var messageBusSettings  = builder.Configuration.GetSection(MessageBusSettings.Key)
            .Get<MessageBusSettings>()!;
        
        cfg.Host(messageBusSettings.Host, messageBusSettings.Port, "/", h => {
            h.Username(messageBusSettings.Username);
            h.Password(messageBusSettings.Password);
        });
        
        cfg.ConfigureEndpoints(context);
        
        EndpointConvention.Map<CheckMessageApiStatus>(new Uri($"exchange:{nameof(CheckMessageApiStatus)}"));
    });
});

builder.Services.AddSingleton<DiagnosticsConfig>();

// Add OpenTelemetry logging provider by calling the WithLogging extension.
builder.Services.AddOpenTelemetry()
    .WithTracing(b => 
        b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(DiagnosticsConfig.ServiceName))
            .AddSource(DiagnosticsConfig.ServiceName)
            .SetSampler(new AlwaysOnSampler())
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter()
    ).WithMetrics(b => 
        b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(DiagnosticsConfig.ServiceName))
            .AddMeter(DiagnosticsConfig.ServiceName)
            
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter())
    .WithLogging(b =>
        b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(DiagnosticsConfig.ServiceName))
            .AddConsoleExporter()
            .AddOtlpExporter());

builder.Logging.AddOpenTelemetry(option =>
{
    option.IncludeScopes = true;
    option.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(DiagnosticsConfig.ServiceName));
    option.AddConsoleExporter()
        .AddOtlpExporter();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.AddTestsEndpoints();

app.Run();