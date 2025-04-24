namespace WeatherTrendAnalyzer.WeatherForecast.Models.Weather
{
    public class WeatherResponse
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public double elevation { get; set; }
        public Units units { get; set; }
        public HourlyData HourlyData { get; set; }
    }
}
