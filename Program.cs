using Meteorological.Enums;
using Meteorological.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Meteorological
{
    public class Program
    {
        private const string URL = "https://opendata-download-metobs.smhi.se/api/version/latest/parameter/";
        private static readonly string urlParameters = MakeParametersString(new List<Parameters> { Parameters.Byvind});
        private const string measuringStations = ""; 
        private const string Format = ".json";
        private static readonly string measuringStation = string.IsNullOrEmpty(measuringStations) ? string.Empty : $"?measuringStations={measuringStations}";

        /// <summary>
        /// Simple console application to present meteorological data from SMHI Meteorology API.
        /// Example Using API: https://opendata-download-metobs.smhi.se/api/version/latest/parameter/21.json
        /// </summary>
        /// <param name="args"></param>
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var weatherReport = new WeatherReport();

            // Build the HttpClient with the base URL (without parameter ids or format)
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request the full relative path including parameters and format
                HttpResponseMessage response = await client.GetAsync(urlParameters + Format + measuringStation);
                if (response.IsSuccessStatusCode)
                {
                    var jsonObject = JObject.Parse(await response.Content.ReadAsStringAsync());
                    weatherReport.Stations = JsonConvert.DeserializeObject<List<WeatherStation>>(((JArray)jsonObject["station"]).ToString());

                    foreach (JObject station in (JArray)jsonObject["station"])
                    {
                        var dataURL = $"https://opendata-download-metobs.smhi.se/api/version/latest/parameter/{urlParameters}/station/{station["key"]}/period/latest-day/data.json";
                        HttpResponseMessage stationResponse = await client.GetAsync(dataURL);

                        if (stationResponse.IsSuccessStatusCode)
                        {
                            var foundStation = weatherReport.Stations.FirstOrDefault(o => o.key == (long)station["key"]);
                            var jsonStationResponseString = await stationResponse.Content.ReadAsStringAsync();
                            var jsonStationResponseObject = JObject.Parse(jsonStationResponseString);
                            foundStation.Data = JsonConvert.DeserializeObject<List<WeatherData>>(((JArray)jsonStationResponseObject["value"]).ToString());
                        }
                        else
                        {
                            Console.WriteLine($"Error: {station["key"]} - {stationResponse.StatusCode} - {stationResponse.ReasonPhrase}");
                           
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return;
                }

                var sSAsA = 1;
            }
        }

        private static string MakeParametersString(IEnumerable<Parameters> parameters)
        {
            if (parameters == null)
            {
                return string.Empty;
            }

            var joined = string.Join(",", parameters.Select(p => ((long)p).ToString()));
            return joined.Replace(" ", "");
        }
    }
}
