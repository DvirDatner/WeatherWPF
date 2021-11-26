using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWPF
{
    class WeatherModel
    {
        public long Dt { get; set; }
        public WeatherIconModel[] Weather { get; set; }
    }
}
