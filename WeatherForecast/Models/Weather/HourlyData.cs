namespace WeatherTrendAnalyzer.WeatherForecast.Models.Weather
{
    public class HourlyData
    {
        public List<string> time { get; set; }
        public List<double> rain { get; set; }
        public List<double> temperature_2m { get; set; }
        public List<int> weather_code { get; set; }
    }
}
