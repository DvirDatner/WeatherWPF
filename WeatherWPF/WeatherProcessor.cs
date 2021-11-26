using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWPF
{
    class WeatherProcessor
    {
        public static async Task<FullWeather> LoadWeather(string location = "Jerusalem")
        {
            var coord = await GetCoord(location);

            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={coord.Coord.Lat}&lon={coord.Coord.Lon}&exclude=alerts,minutely&appid=9b2be1d859f6fbc20dcf5bf926bd7b26&units=metric";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    FullWeather fullWeather = await response.Content.ReadAsAsync<FullWeather>();

                    return fullWeather;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private static async Task<CoordinatesModel> GetCoord(string location)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid=9b2be1d859f6fbc20dcf5bf926bd7b26";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    CoordinatesModel coord = await response.Content.ReadAsAsync<CoordinatesModel>();

                    return coord;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
