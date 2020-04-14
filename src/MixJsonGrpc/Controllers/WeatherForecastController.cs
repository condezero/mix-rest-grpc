using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MixJsonGrpc.Services;

namespace MixJsonGrpc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IWeatherService weatherService, ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _weatherService.GetWeather(new Google.Protobuf.WellKnownTypes.Empty(), null);
            return Ok(response);
        }
    }
}
