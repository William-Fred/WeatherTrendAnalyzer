using WeatherTrendAnalyzer.Components;
using WeatherTrendAnalyzer.WeatherForecast.Services;

namespace WeatherTrendAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            //Add in memory cache 
            builder.Services.AddMemoryCache();

            //Register weather service 
            builder.Services.AddHttpClient<WeatherForecastService>();

            //Register calculate service
            builder.Services.AddSingleton<CalculateWeatherService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
