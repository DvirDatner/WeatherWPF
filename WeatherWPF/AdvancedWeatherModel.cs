using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWPF
{
    class AdvancedWeatherModel : WeatherModel
    {
        public AdvancedTempModel Temp { get; set; }
        public BasicTempModel Feels_like { get; set; }
    }
}
