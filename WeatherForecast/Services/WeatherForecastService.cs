using System.Text.Json;
using WeatherTrendAnalyzer.WeatherForecast.Models.Weather;

namespace WeatherTrendAnalyzer.WeatherForecast.Services
{
    public class WeatherForecastService
    {
        public const string WeatherForecastBaseUrl = "https://api.open-meteo.com/v1/forecast?";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResponse> GetWeatherForecastAsync(int days)
        {
            var weatherForecastUrl = $"{WeatherForecastBaseUrl}latitude=57.7072&longitude=11.9668&hourly=temperature_2m,rain&timezone=Europe%2FBerlin&forecast_days={days}&models=dmi_seamless";

            var response = await _httpClient.GetAsync(weatherForecastUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching weather data: {response.StatusCode}");
            }

            //If the deserialized result is null, we return default instance instead of return null.
            return await response.Content.ReadFromJsonAsync<WeatherResponse>(_jsonSerializerOptions) ?? new WeatherResponse();
        }
    }
}
