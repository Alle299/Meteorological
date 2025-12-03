using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteorological.Models
{
    public class WeatherStation
    {
        public long key { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string OwnerCategory { get; set; }
        public string MeasuringStations { get; set; }
        public long Height { get; set; }
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public bool Active { get; set; }
        public long From { get; set; }
        public long To { get; set; }
        public List<WeatherData> Data { get; set; } = new List<WeatherData>();

    }
}
