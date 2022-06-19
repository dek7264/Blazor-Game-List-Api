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
        private IDatabaseConnectionSettings DbSettings { get; }
        public ISendEndpointProvider SendEndpointProvider { get; }
        public IConfiguration Configuration { get; }

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDatabaseConnectionSettings settings, 
            ISendEndpointProvider sendEndpointProvider, IConfiguration configuration)
        {
            Logger = logger;
            DbSettings = settings;
            SendEndpointProvider = sendEndpointProvider;
            Configuration = configuration;
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
            var query = new GetTestInfoDatabaseQuery(DbSettings);
            return await query.GetTestInfo();
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