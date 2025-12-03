using System;
using Newtonsoft.Json;

namespace Meteorological.Models
{
    public class WeatherData
    {
        //[JsonConverter(typeof(UnixEpochLongToDateTimeConverter))]
        public DateTime Date { get; set; }

        public double Value { get; set; }
        public string Quality { get; set; }
    }
}
