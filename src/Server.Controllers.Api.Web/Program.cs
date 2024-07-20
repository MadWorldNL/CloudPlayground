using MadWorldNL.CloudPlayground.Endpoints;
using MadWorldNL.CloudPlayground.MessageBus;
using MadWorldNL.CloudPlayground.Tests;
using MassTransit;
using RabbitMQ.Client;

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
        .Endpoint(e => e.Name = "messagebus-status");
    
    x.AddRequestClient<CheckMessageBusStatus>(new Uri("exchange:messagebus-status"));
    
    x.UsingRabbitMq((context,cfg) =>
    {
        var messageBusSettings  = builder.Configuration.GetSection(MessageBusSettings.Key)
            .Get<MessageBusSettings>()!;
        
        cfg.Host(messageBusSettings.Host, messageBusSettings.Port, "/", h => {
            h.Username(messageBusSettings.Username);
            h.Password(messageBusSettings.Password);
        });
        
        cfg.ConfigureEndpoints(context);
        
        EndpointConvention.Map<CheckMessageApiStatus>(new Uri($"rabbitmq://{messageBusSettings.Host}:{messageBusSettings.Port}/{nameof(CheckMessageApiStatus)}"));
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddTestsEndpoints();

app.Run();