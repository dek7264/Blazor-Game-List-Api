using Game_List_Api.Infrastructure;
using MassTransit;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMqConfiguration:HostName"], "/", host =>
        {
            host.Username(builder.Configuration["RabbitMqConfiguration:UserName"]);
            host.Password(builder.Configuration["RabbitMqConfiguration:Password"]);
        });

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDatabaseContext<IHerokuPostgresDatabaseContext, HerokuPostgresDatabaseContext>(typeof(HerokuPostgresDatabaseContext).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
