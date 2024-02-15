using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;

namespace Retry.CircuitBreaker.Polly.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService weatherService;
        public WeatherForecastController(IWeatherService weatherService)
        {
            this.weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherData([FromQuery] string cityName)
        {
            try
            {
                return new JsonResult(await weatherService.GetWeatherData(cityName));
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine("Circuit is open now");
            }
            return null;
        }
    }
}
