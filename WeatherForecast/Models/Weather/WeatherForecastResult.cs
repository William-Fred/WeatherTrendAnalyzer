namespace WeatherTrendAnalyzer.WeatherForecast.Models.Weather
{
    public class WeatherForecastResult
    {
        public double AverageTemp { get; set; }
        public int TotalRainHours { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public DateTime Date { get; set; }
        public List<DailyForecastResult>? DailyForecast { get; set; }
        public string TemperatureUnit { get; set; }
        public string RainUnit { get; set; }

    }
}
