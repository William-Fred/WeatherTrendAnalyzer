namespace WeatherTrendAnalyzer.WeatherForecast.Models.Weather
{
    public sealed class DailyForecastResult
    {
        public DateTime Date { get; set; }
        public double AverageTemp { get; set; }
        public int TotalRainDays { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
    }
}
