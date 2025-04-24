using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherForecastService> _logger;
        public WeatherForecastService(HttpClient httpClient, ILogger<WeatherForecastService> logger, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cache = cache;
        }

        public async Task<GeoLocation> GetCoordinatesForCityAsync(string cityName)
        {
            string cacheKey = $"geo_{cityName}";

            //Returnera cachat värde och logg att vi hämtar från cache
            if (_cache.TryGetValue(cacheKey, out GeoLocation? cachedLocation))
            {
                _logger.LogInformation("Hämtar koordinater för stad {name} från cache.", cityName);
                return cachedLocation;
            }

            var geoLocationUrl = $"{GeoLocationBaseUrl}?name={cityName}&count=1&language=sv&format=json"; ;

            var response = await _httpClient.GetAsync(geoLocationUrl);

            //Annars logga om det är från API
            _logger.LogInformation("Hämtade koordinater för {name} från API.", cityName);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching geolocation data: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<GeoLocationResponse>(_jsonSerializerOptions);
            var location = result?.Results?.FirstOrDefault() ?? new GeoLocation();

            // Cache sätts till 10 minutes
            _cache.Set(cacheKey, location, TimeSpan.FromMinutes(10));

            return location;
        }

        public async Task<WeatherResponse> GetWeatherForecastAsync(int days, double latitude, double longitude, string timeZone)
        {
            var cacheKey = $"$forecast_{latitude}_{longitude}_{days}_{timeZone}";

            if (_cache.TryGetValue(cacheKey, out WeatherResponse? cachedForecast))
            {
                _logger.LogInformation("Hämtade väderdata för [{Lat}, {Lon}] {Days} dagar från cache.", latitude, longitude, days);
                return cachedForecast;
            }

            var weatherForecastUrl =
                $"{WeatherForecastBaseUrl}latitude={latitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}" +
                $"&hourly=rain,temperature_2m,weather_code" +
                $"&timezone={timeZone}" +
                $"&forecast_days={days}" +
                $"&models=dmi_seamless";

            var response = await _httpClient.GetAsync(weatherForecastUrl);

            _logger.LogInformation("Hämtade väderdata för [{Lat}, {Lon}] {Days} dagar från API.", latitude, longitude, days);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching weather data: {response.StatusCode}");
            }

            var weatherResponse = await response.Content.ReadFromJsonAsync<WeatherResponse>(_jsonSerializerOptions);

            _cache.Set(cacheKey, weatherResponse, TimeSpan.FromMinutes(10));

            return weatherResponse ?? new WeatherResponse();
        }
    }
}
