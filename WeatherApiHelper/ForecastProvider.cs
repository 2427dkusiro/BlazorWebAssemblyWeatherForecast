using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherApiHelper
{
    public static class ForecastProvider
    {
        public static async Task<WeatherForecast> FetchForecast(HttpClient httpClient, int positionId)
        {
            const string url = @"https://weather.tsukumijima.net/api/forecast";
            string query = $"?city={positionId.ToString("D6")}";

            return await httpClient.GetFromJsonAsync<WeatherForecast>(url + query);
        }
    }

    public class WeatherForecast
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("forecasts")]
        public WeatherForecastBody[] Forecasts { get; set; }
    }

    public class WeatherForecastBody
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("telop")]
        public string Telop { get; set; }

        [JsonPropertyName("temperature")]
        public TemperatureOfDay Temperature { get; set; }
    }

    public class TemperatureOfDay
    {
        [JsonPropertyName("min")]
        public Temperature Min { get; set; }

        [JsonPropertyName("max")]
        public Temperature Max { get; set; }
    }

    public class Temperature
    {
        [JsonPropertyName("celsius")]
        public decimal? Celsius { get; set; }

        [JsonPropertyName("fahrenheit")]
        public decimal? Fahrenheit { get; set; }
    }
}
