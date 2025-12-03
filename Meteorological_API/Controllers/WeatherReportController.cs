using Meteorological.Enums;
using Meteorological.Models;
using Meteorological_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Meteorological_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherReportController : ControllerBase
    {
        private readonly IWeatherReportService _weatherReportService;
        private readonly ILogger<WeatherReportController> _logger;

        /// <summary>
        /// Weather reports controller
        /// </summary>
        /// <param name="weatherReportService"></param>
        /// <param name="logger"></param>
        public WeatherReportController(IWeatherReportService weatherReportService,
                                       ILogger<WeatherReportController> logger)
        {
            _weatherReportService = weatherReportService;
            _logger = logger;
        }

        /// <summary>
        /// Get Wind reports from one or all weatherstations. Powered by SMHI.
        /// </summary>
        /// <remarks>
        /// <br /> Source: "https://opendata-download-metobs.smhi.se/api/version/latest/parameter/"
        /// <br /> 
        /// <br /> Example: <c>/WeatherReport/wind?stationKey=188790</c> to query a single station.
        /// </remarks>
        /// <returns>A list with a single <see cref="WeatherReport"/> for the requested parameter/station.</returns>
        [HttpGet("wind", Name = "GetWeatherReportWind")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherReport>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<IEnumerable<WeatherReport>>> GetByWind([FromQuery] long? stationKey, [FromQuery] bool latestDay)
        {
            try
            {
                // Normaly I put all parameter validation in the controller, but in this case when default falback parameters are involved,
                // I felt it prudent to have that in the service.
                var result = await _weatherReportService.GetDataByParameter(Parameter.Byvind, stationKey, latestDay);

                if (result == null || result.Stations.Count() == 0)
                {
                    // No data available for the parameter -> 404 Not Found with ProblemDetails
                    _logger.LogInformation("GetByWind: no data returned for parameter {Parameter}", Parameter.Byvind);

                    var notFoundProblem = new ProblemDetails
                    {
                        Title = "Data not found",
                        Detail = $"No weather report available for parameter '{Parameter.Byvind}'.",
                        Status = StatusCodes.Status404NotFound,
                        Instance = HttpContext?.Request?.Path
                    };

                    return NotFound(notFoundProblem);
                }

                var weatherReportList = new List<WeatherReport> { result };
                return Ok(weatherReportList);
            }
            catch (HttpRequestException ex)
            {
                // Specific handling for external HTTP failures -> 502 Bad Gateway
                _logger.LogError(ex, "GetByWind: external request failed while retrieving weather report for {Parameter}", Parameter.Byvind);

                var externalProblem = new ProblemDetails
                {
                    Title = "External service error",
                    Detail = "Failed to retrieve data from an upstream service. Try again later.",
                    Status = StatusCodes.Status502BadGateway,
                    Instance = HttpContext?.Request?.Path
                };

                return StatusCode(externalProblem.Status ?? StatusCodes.Status502BadGateway, externalProblem);
            }
            catch (Exception ex)
            {
                // Unexpected/unhandled exceptions -> 500 Internal Server Error
                _logger.LogError(ex, "GetByWind: unexpected error while retrieving weather report for {Parameter}", Parameter.Byvind);

                var internalProblem = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Detail = "An unexpected error occurred while processing the request.",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext?.Request?.Path
                };

                return StatusCode(internalProblem.Status ?? StatusCodes.Status500InternalServerError, internalProblem);
            }
        }

        /// <summary>
        /// Get Temperature reports from one or all weatherstations. Powered by SMHI
        /// </summary>
        /// <remarks>
        /// <br /> Source: "https://opendata-download-metobs.smhi.se/api/version/latest/parameter/"
        /// <br /> 
        /// <br /> Example: <c>/WeatherReport/wind?stationKey=188790</c> to query a single station.
        /// </remarks>
        /// <returns>A list with a single <see cref="WeatherReport"/> for the requested parameter/station.</returns>
        [HttpGet("temperature", Name = "GetWeatherReportTemperature")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherReport>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status502BadGateway, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<IEnumerable<WeatherReport>>> GetByTemperature([FromQuery] long? stationKey, [FromQuery] bool latestDay)
        {
            try
            {
                // Normaly I put all parameter validation in the controller, but in this case when default falback parameters are involved,
                // I felt it prudent to have that in the service.
                var result = await _weatherReportService.GetDataByParameter(Parameter.Lufttemperatur_Nu, stationKey, latestDay);

                if (result == null)
                {
                    // No data available for the parameter -> 404 Not Found with ProblemDetails
                    _logger.LogInformation("GetByTemperature: no data returned for parameter {Parameter}", Parameter.Lufttemperatur_Nu);

                    var notFoundProblem = new ProblemDetails
                    {
                        Title = "Data not found",
                        Detail = $"No weather report available for parameter '{Parameter.Lufttemperatur_Nu}'.",
                        Status = StatusCodes.Status404NotFound,
                        Instance = HttpContext?.Request?.Path
                    };

                    return NotFound(notFoundProblem);
                }

                var weatherReportList = new List<WeatherReport> { result };
                return Ok(weatherReportList);
            }
            catch (HttpRequestException ex)
            {
                // Specific handling for external HTTP failures -> 502 Bad Gateway
                _logger.LogError(ex, "GetByTemperature: external request failed while retrieving weather report for {Parameter}", Parameter.Lufttemperatur_Nu);

                var externalProblem = new ProblemDetails
                {
                    Title = "External service error",
                    Detail = "Failed to retrieve data from an upstream service. Try again later.",
                    Status = StatusCodes.Status502BadGateway,
                    Instance = HttpContext?.Request?.Path
                };

                return StatusCode(externalProblem.Status ?? StatusCodes.Status502BadGateway, externalProblem);
            }
            catch (Exception ex)
            {
                // Unexpected/unhandled exceptions -> 500 Internal Server Error
                _logger.LogError(ex, "GetByTemperature: unexpected error while retrieving weather report for {Parameter}", Parameter.Lufttemperatur_Nu);

                var internalProblem = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Detail = "An unexpected error occurred while processing the request.",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = HttpContext?.Request?.Path
                };

                return StatusCode(internalProblem.Status ?? StatusCodes.Status500InternalServerError, internalProblem);
            }
        }
    }
}
