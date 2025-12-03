using Meteorological.Enums;
using Meteorological.Models;


namespace Meteorological_API.Service
{
    public interface IWeatherReportService
    {
        /// <summary>
        /// Interface for the GetDataByParameter method
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="stationKey"></param>
        /// <param name="latestDay"></param>
        /// <returns></returns>
        Task<WeatherReport?> GetDataByParameter(Parameter parameter, long? stationKey, bool latestDay);
    }
}
