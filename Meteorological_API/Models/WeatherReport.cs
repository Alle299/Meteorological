using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meteorological.Models
{
    public class WeatherReport
    {
        public List<WeatherStation> Stations { get; set; } = new List<WeatherStation>();
    }
}
