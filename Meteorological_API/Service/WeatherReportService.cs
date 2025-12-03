using Meteorological.Enums;
using Meteorological.Models;
using Meteorological_API.Service.Helpers;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Meteorological_API.Service
{
    /// <summary>
    /// Sevice to manage the WatherReport api calls
    /// </summary>
    public class WeatherReportService : IWeatherReportService
    {
        private const string URL = "https://opendata-download-metobs.smhi.se/api/version/latest/parameter/";
        private const string Format = ".json";

        /// <summary>
        /// Service function for specific api call using Parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public async Task<WeatherReport?> GetDataByParameter(Parameter parameter, long? stationKey, bool latestDay)
        {
            var weatherReport = new WeatherReport();

            string measuringStation = MeassuringStation.GetValueUrlAugmentationOrEmpty(stationKey);
            string scope = Scope.GetValueUrlAugmentationOrEmpty(latestDay);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request metadata for the parameter (stations list)
                HttpResponseMessage response = await client.GetAsync((long)parameter + measuringStation + Format);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return null;
                }

                var contentString = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(contentString);


                if (stationKey == null || stationKey == 0)
                {
                    // if looking at all stations
                    var stationsToken = jsonObject["station"] as JArray;
                    var stations = stationsToken != null
                        ? stationsToken.ToObject<List<WeatherStation>>()
                        : new List<WeatherStation>();

                    weatherReport.Stations = stations ?? new List<WeatherStation>();
                } else
                {
                    // if looking for one station
                    weatherReport.Stations = new List<WeatherStation> { jsonObject.ToObject<WeatherStation>()};
                }

                // For each strongly-typed WeatherStation, fetch latest-day data
                foreach (var station in weatherReport.Stations)
                {
                    if (station == null)
                        continue;

                    var dataRelativePath = $"{(long)parameter}/station/{station.key}/period/{scope}/data{Format}";
                    HttpResponseMessage stationResponse;

                    try
                    {
                        stationResponse = await client.GetAsync(dataRelativePath);

                        if (!stationResponse.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Error: {station.key} - {stationResponse.StatusCode} - {stationResponse.ReasonPhrase}");
                            continue;
                        }

                        var stationContent = await stationResponse.Content.ReadAsStringAsync();
                        var stationObject = JObject.Parse(stationContent);

                        var valuesToken = stationObject["value"] as JArray;
                        station.Data = valuesToken != null
                            ? valuesToken.ToObject<List<WeatherData>>()
                            : new List<WeatherData>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error requesting station {station.key}: {ex.Message}");
                        continue;
                    }
                }

                return weatherReport;
            }
        }
    }
}
