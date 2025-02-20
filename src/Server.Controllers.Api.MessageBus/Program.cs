using MadWorldNL.CloudPlayground.MessageBus;
using MadWorldNL.CloudPlayground.Tests;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<CheckMessageApiStatusConsumer>();
    x.AddConsumer<CheckMultiMessageApiStatusConsumer>();
    
    x.UsingRabbitMq((context,cfg) =>
    {
        var messageBusSettings  = builder.Configuration.GetSection(MessageBusSettings.Key)
            .Get<MessageBusSettings>()!;
        
        cfg.Host(messageBusSettings.Host, messageBusSettings.Port, "/", h => {
            h.Username(messageBusSettings.Username);
            h.Password(messageBusSettings.Password);
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.Run();