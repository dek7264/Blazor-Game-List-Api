using Game_List_Api.Infrastructure;
using Game_List_Api.Infrastructure.DatabaseQueries;
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDatabaseConnectionSettings settings)
        {
            Logger = logger;
            DbSettings = settings;
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
    }
}