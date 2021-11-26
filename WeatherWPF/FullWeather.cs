using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWPF
{
    class FullWeather
    {
        public BasicWeatherModel Current { get; set; }
        public BasicWeatherModel[] Hourly { get; set; }
        public AdvancedWeatherModel[] Daily { get; set; }
    }
}
