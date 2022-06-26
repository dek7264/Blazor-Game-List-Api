using Game_List_Api.Infrastructure;
using Game_List_Api.Infrastructure.DatabaseQueries;
using GameListApiWorker;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Game_List_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ILogger<WeatherForecastController> Logger { get; }
        private ISendEndpointProvider SendEndpointProvider { get; }
        private IConfiguration Configuration { get; }
        private IHerokuPostgresDatabaseContext DatabaseContext { get; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            ISendEndpointProvider sendEndpointProvider, IConfiguration configuration, IHerokuPostgresDatabaseContext databaseContext)
        {
            Logger = logger;
            SendEndpointProvider = sendEndpointProvider;
            Configuration = configuration;
            DatabaseContext = databaseContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Test")]
        public async Task<IReadOnlyList<int>> TestFromDb()
        {
            return await DatabaseContext.ExecuteQuery(new GetTestInfoDatabaseQuery());
        }

        [HttpPost("PublishRabbitMqMessage")]
        public async Task PostRabbitMqMessage()
        {
            var endpoint = await SendEndpointProvider.GetSendEndpoint(new Uri($"queue:{Configuration["RabbitMqConfiguration:QueueName"]}"));
            await endpoint.Send(new TestMessage("publish-test", 456));
        }
    }
}

namespace GameListApiWorker
{
    public record TestMessage(string TestString, int TestInt);
}