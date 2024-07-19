using MadWorldNL.CloudPlayground.Endpoints;
using MadWorldNL.CloudPlayground.Tests;
using MassTransit;
using MassTransit.MultiBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CheckMessageBusStatusConsumer>()
        .Endpoint(e => e.Name = "messagebus-status");
    
    x.AddRequestClient<CheckMessageBusStatus>(new Uri("exchange:messagebus-status"));
    
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
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