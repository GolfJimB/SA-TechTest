using Common.Clients;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly MeterologyClient meterologyClient;
        private readonly MeterologyService meterologyService;

        public WeatherForecastController(ILogger<WeatherForecastController> _logger, MeterologyClient _meterologyClient,
            MeterologyService _meterologyService)
        {
            logger = _logger;
            meterologyClient = _meterologyClient;
            meterologyService = _meterologyService;
        }

        [HttpGet(Name = "GetStationWeather")]
        public async Task<ActionResult<StationWeatherData>> GetStationWeather(string weatherStationCode)
        {
            var url = $"http://www.bom.gov.au/fwo/IDS60901/IDS60901.{weatherStationCode}.json";
            var data = meterologyClient.GetMeteorologyData(url);
            return Ok(data);
        }

        [HttpGet(Name = "GetStationWeatherDatum")]
        public async Task<ActionResult<string>> GetStationWeatherDatum(string weatherStationCode, DataTypeEnum dataType)
        {
            var url = $"http://www.bom.gov.au/fwo/IDS60901/IDS60901.{weatherStationCode}.json";
            var data = meterologyService.GetStationDatum(url, dataType);
            return Ok(data);
        }

        ///Explanation
        ///wanted to keep it strongly type so made two endpoints with the latter just returning a string
        ///could've made dedicated endpoints per data type. i.e. GetAirTemperatureAt
        ///
        ///Decided to use enums to help the caller pick the type of data they want, could've used reflection but that's dangerous and slow
        ///Using a string straight into the url like that is a dangerous attack vector too. Would need more validation and penetration testing
        ///
        ///Went to the effort of implementing some SOLID archiecture, nice and extensible and testable, would split the common lib down into more libs in future
    }
}
