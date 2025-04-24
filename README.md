# WeatherTrendAnalyzer
-En enkel väderapplikation som hämtar och visualiserar väderdata timme för timme baserat på geografisk plats. Appen använder sig av Open-Meteo Geocoding API och Open-Meteo DMI Forecast API för att hämta den data som presenteras.

## Projektstruktur

```plaintext
WeatherTrendAnalyzer/
│
├── WeatherTrendAnalyzer.Client/             # Blazor WebAssembly-klient
│   ├── Pages/
│   │   └── Weather.razor                     # UI för att visa väderprognos
│   ├── Services/
│   │   ├── WeatherForecastService.cs        # Hämtar väderdata från API
│   │   └── CalculateWeatherService.cs       # Uträkningar för att visa upp korrekt data
│
├── WeatherForecast/                         # Modeller
│   ├── Models/
│   │   ├── WeatherResponse.cs               # Rootobjekt från väder-API:t
│   │   ├── HourlyData.cs                    # Timvis väderdata
│   │   ├── HourlyUnits.cs                   # Enheter för timvis data
│   │   ├── WeatherForecastResult.cs         # Beräknad väderprognos (genomsnitt, max, min)
│   │   └── DailyForecastResult.cs           # Prognosdata för varje enskild dag
│
├── WeatherForecast.sln                      # Lösningsfil
└── README.md                                # Dokumentation

## Att köra applikationen
- Öppna en terminal eller kommandoprompt.
- Navigera till projektmappen där .csproj-filen finns.

Kör kommandot:
-dotnet run 

-Gå mot Localhost:xxxx som visas i terminalen. 

Väl i applikationen:
- Navigera till /weather
- Skriv in stad
- Tryck på knappen "hämta väder
### Klona repot

git clone https://github.com/William-Fred/WeatherTrendAnalyzer.git
cd 
