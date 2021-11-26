using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWPF
{
    class CoordinatesModel
    {
        public Coord Coord { get; set; }
    }

    class Coord
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
