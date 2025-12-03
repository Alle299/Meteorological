using System;
using System.Collections.Generic;

namespace Meteorological.Models
{
    public class WeatherReport
    {
        public List<WeatherStation> Stations { get; set; } = new List<WeatherStation>();
    }
}
