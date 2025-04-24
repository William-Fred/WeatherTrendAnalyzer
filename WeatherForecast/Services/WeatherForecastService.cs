using System.Globalization;
using System.Text.Json;
using WeatherTrendAnalyzer.WeatherForecast.Models.GeoLocation;
using WeatherTrendAnalyzer.WeatherForecast.Models.Weather;

namespace WeatherTrendAnalyzer.WeatherForecast.Services
{
    public class WeatherForecastService
    {
        public const string WeatherForecastBaseUrl = "https://api.open-meteo.com/v1/forecast?";
        public const string GeoLocationBaseUrl = "https://geocoding-api.open-meteo.com/v1/search";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GeoLocation> GetCoordinatesForCityAsync(string cityName)
        {
            var geoLocationUrl = $"{GeoLocationBaseUrl}?name={cityName}&count=1&language=sv&format=json"; ;

            var response = await _httpClient.GetAsync(geoLocationUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching geolocation data: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<GeoLocationResponse>(_jsonSerializerOptions);
            return result?.Results?.FirstOrDefault() ?? new GeoLocation();
        }

        public async Task<WeatherResponse> GetWeatherForecastAsync(int days, double latitude, double longitude, string timeZone)
        {
            var weatherForecastUrl =
                $"{WeatherForecastBaseUrl}latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&hourly=rain,temperature_2m,weather_code" +
                $"&timezone={timeZone}" +
                $"&forecast_days={days}" +
                $"&models=dmi_seamless";

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
