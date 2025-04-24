using WeatherTrendAnalyzer.WeatherForecast.Models.Weather;

namespace WeatherTrendAnalyzer.WeatherForecast.Services
{
    public class CalculateWeatherService
    {
        public WeatherForecastResult CalculateWeatherForecast(WeatherResponse weather)
        {
            return new WeatherForecastResult
            {
                AverageTemp = GetAverageTemperature(weather),
                TotalRainHours = GetRainyHoursCount(weather),
                MaxTemp = GetMaxTemperature(weather),
                MinTemp = GetMinTemperature(weather),
                DailyForecast = GroupForecastByDay(weather),
                TemperatureUnit = weather?.hourly_units?.temperature_2m,
                RainUnit = weather?.hourly_units?.rain
            };
        }

        public double GetAverageTemperature(WeatherResponse weather)
        {
            return weather?.hourly?.temperature_2m?.Average() ?? 0;
        }

        public int GetRainyHoursCount(WeatherResponse weather)
        {
            return weather?.hourly?.rain?.Count(r => r > 0) ?? 0;
        }

        public double GetMaxTemperature(WeatherResponse weather)
        {
            return weather?.hourly?.temperature_2m?.Max() ?? 0;
        }

        public double GetMinTemperature(WeatherResponse weather)
        {
            return weather?.hourly?.temperature_2m?.Min() ?? 0;
        }

        public List<DailyForecastResult> GroupForecastByDay(WeatherResponse weather)
        {
            var berlinEuropeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");

            var grouped = weather.hourly.time
                .Select((time, index) => new
                {
                    Time = DateTime.Parse(time),
                    Temperature = weather.hourly.temperature_2m[index],
                    Rain = weather.hourly.rain[index]
                })
                .GroupBy(data =>
                {
                    var berlinTime = TimeZoneInfo.ConvertTime(data.Time, berlinEuropeTimeZone);
                    return berlinTime.Date;
                })
                .Select(group => new DailyForecastResult
                {
                    Date = group.Key,
                    AverageTemp = group.Average(x => x.Temperature),
                    TotalRainDays = group.Count(x => x.Rain > 0),
                    MaxTemperature = group.Max(x => x.Temperature),
                    MinTemperature = group.Min(x => x.Temperature)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return grouped;
        }
    }
}
