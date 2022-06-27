using Game_List_Api.Controllers.Sandbox.DbRequest;
using Game_List_Api.Controllers.Sandbox.RabbitMqRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Game_List_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SandboxController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ILogger<SandboxController> Logger { get; }
        private IMediator Mediator { get; }

        public SandboxController(ILogger<SandboxController> logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
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
        public Task<IReadOnlyList<int>> TestFromDb()
        {
            return Mediator.Send(new TestDbRequest());
        }

        [HttpPost("PublishRabbitMqMessage")]
        public Task PostRabbitMqMessage(PublishInput input)
        {
            return Mediator.Publish(new SendRabbitMqMessageRequest(input.TestString, input.TestInt));
        }

        public record PublishInput(string TestString, int TestInt);
    }
}