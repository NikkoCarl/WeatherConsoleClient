using WeatherConsoleClient.Application.DTOs;
using WeatherConsoleClient.Application.Interfaces;

namespace WeatherConsoleClient.Presentation;

public class ConsoleMenu
{
    private readonly IWeatherService _weatherService;
    private readonly IWeatherFormatter _weatherFormatter;

    public ConsoleMenu(
        IWeatherService weatherService,
        IWeatherFormatter weatherFormatter)
    {
        _weatherService = weatherService;
        _weatherFormatter = weatherFormatter;
    }

    public async Task RunAsync(
        CancellationToken cancellationToken)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("       WEATHER CONSOLE CLIENT");
            Console.WriteLine("========================================");
            Console.WriteLine();
            Console.WriteLine("1. Current Weather");
            Console.WriteLine("2. 5-Day / 3-Hour Forecast");
            Console.WriteLine("3. Weather Dashboard");
            Console.WriteLine("0. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                if (choice == "1")
                {
                    await ShowCurrentWeatherAsync(cancellationToken);
                }
                else if (choice == "2")
                {
                    await ShowForecastAsync(cancellationToken);
                }
                else if (choice == "3")
                {
                    await ShowDashboardAsync(cancellationToken);
                }
                else if (choice == "0")
                {
                    Console.WriteLine("Goodbye!");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong.");
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }
    }

    private string GetTemperatureUnit()
    {
        Console.WriteLine();
        Console.WriteLine("Temperature Unit:");
        Console.WriteLine("1. Celsius");
        Console.WriteLine("2. Fahrenheit");
        Console.Write("Enter choice: ");

        string? choice = Console.ReadLine();

        if (choice == "2")
        {
            return "F";
        }

        return "C";
    }

    private decimal ConvertTemperature(
        decimal celsius,
        string unit)
    {
        if (unit == "F")
        {
            return (celsius * 9 / 5) + 32;
        }

        return celsius;
    }

    private async Task ShowCurrentWeatherAsync(
        CancellationToken cancellationToken)
    {
        Console.Write("Enter city: ");

        string? city = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("City is required.");
            return;
        }

        string unit = GetTemperatureUnit();

        CurrentWeatherDto? weather =
            await _weatherService.GetCurrentWeatherAsync(
                city,
                cancellationToken);

        if (weather == null)
        {
            Console.WriteLine("City not found.");
            return;
        }

        decimal temperature =
            ConvertTemperature(
                weather.Main.Temperature,
                unit);

        decimal feelsLike =
            ConvertTemperature(
                weather.Main.FeelsLike,
                unit);

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("CURRENT WEATHER");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine($"City        : {weather.Name}");
        Console.WriteLine(
            $"Temperature : {temperature:F1} °{unit}");
        Console.WriteLine(
            $"Feels Like  : {feelsLike:F1} °{unit}");
        Console.WriteLine(
            $"Humidity    : {weather.Main.Humidity}%");
        Console.WriteLine(
            $"Pressure    : {weather.Main.Pressure} hPa");

        if (weather.Weather.Count > 0)
        {
            Console.WriteLine(
                $"Condition   : {weather.Weather[0].Description}");
        }

        Console.WriteLine(
            $"Wind Speed  : {weather.Wind.Speed:F1} m/s");

        if (weather.Main.Temperature > 35)
        {
            Console.WriteLine();
            Console.WriteLine("!!! HOT WEATHER ALERT !!!");
            Console.WriteLine("Temperature is above 35 °C.");
        }
    }

    private async Task ShowForecastAsync(
        CancellationToken cancellationToken)
    {
        Console.Write("Enter city: ");

        string? city = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("City is required.");
            return;
        }

        ForecastDto? forecast =
            await _weatherService.GetForecastAsync(
                city,
                cancellationToken);

        if (forecast == null)
        {
            Console.WriteLine("City not found.");
            return;
        }

        string unit = GetTemperatureUnit();

        Console.WriteLine();
        Console.WriteLine("Forecast Filter:");
        Console.WriteLine("1. All");
        Console.WriteLine("2. Today");
        Console.WriteLine("3. Tomorrow");
        Console.Write("Enter choice: ");

        string? filterChoice = Console.ReadLine();

        DateTime today = DateTime.Today;
        DateTime tomorrow = today.AddDays(1);

        List<ForecastItemDto> filteredItems =
            new List<ForecastItemDto>();

        foreach (ForecastItemDto item in forecast.Items)
        {
            if (!DateTime.TryParse(
                item.DateTimeText,
                out DateTime forecastDate))
            {
                continue;
            }

            if (filterChoice == "1")
            {
                filteredItems.Add(item);
            }
            else if (
                filterChoice == "2" &&
                forecastDate.Date == today)
            {
                filteredItems.Add(item);
            }
            else if (
                filterChoice == "3" &&
                forecastDate.Date == tomorrow)
            {
                filteredItems.Add(item);
            }
        }

        Console.WriteLine();

        if (filteredItems.Count == 0)
        {
            Console.WriteLine(
                "No forecast data available for this selection.");
            return;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("FORECAST");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine($"City: {forecast.City.Name}");
        Console.WriteLine();

        bool rainAlertShown = false;

        foreach (ForecastItemDto item in filteredItems)
        {
            string condition =
                item.Weather.Count > 0
                    ? item.Weather[0].Description
                    : "Unknown";

            decimal rain =
                item.RainProbability * 100;

            decimal temperature =
                ConvertTemperature(
                    item.Main.Temperature,
                    unit);

            Console.WriteLine(
                $"{item.DateTimeText}  " +
                $"{temperature:F1} °{unit}  " +
                $"{condition}  " +
                $"{rain:F0}%");

            if (rain >= 60)
            {
                rainAlertShown = true;
            }
        }

        Console.WriteLine();

        if (rainAlertShown)
        {
            Console.WriteLine("!!! RAIN ALERT !!!");
            Console.WriteLine(
                "There is a 60% or higher chance of rain.");
        }

        Console.WriteLine();
        Console.WriteLine("FORECAST SUMMARY");
        Console.WriteLine("----------------------------------------");

        decimal highestTemperature =
            filteredItems.Max(
                item => item.Main.Temperature);

        decimal lowestTemperature =
            filteredItems.Min(
                item => item.Main.Temperature);

        decimal averageTemperature =
            filteredItems.Average(
                item => item.Main.Temperature);

        decimal highestRainProbability =
            filteredItems.Max(
                item => item.RainProbability) * 100;

        highestTemperature =
            ConvertTemperature(
                highestTemperature,
                unit);

        lowestTemperature =
            ConvertTemperature(
                lowestTemperature,
                unit);

        averageTemperature =
            ConvertTemperature(
                averageTemperature,
                unit);

        Console.WriteLine(
            $"Highest Temperature : {highestTemperature:F1} °{unit}");

        Console.WriteLine(
            $"Lowest Temperature  : {lowestTemperature:F1} °{unit}");

        Console.WriteLine(
            $"Average Temperature : {averageTemperature:F1} °{unit}");

        Console.WriteLine(
            $"Highest Rain Chance : {highestRainProbability:F0}%");
    }

    private async Task ShowDashboardAsync(
        CancellationToken cancellationToken)
    {
        Console.Write("Enter city: ");

        string? city = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(city))
        {
            Console.WriteLine("City is required.");
            return;
        }

        string unit = GetTemperatureUnit();

        CurrentWeatherDto? weather =
            await _weatherService.GetCurrentWeatherAsync(
                city,
                cancellationToken);

        ForecastDto? forecast =
            await _weatherService.GetForecastAsync(
                city,
                cancellationToken);

        if (weather == null || forecast == null)
        {
            Console.WriteLine("City not found.");
            return;
        }

        decimal temperature =
            ConvertTemperature(
                weather.Main.Temperature,
                unit);

        decimal feelsLike =
            ConvertTemperature(
                weather.Main.FeelsLike,
                unit);

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("WEATHER DASHBOARD");
        Console.WriteLine("========================================");

        Console.WriteLine();
        Console.WriteLine($"City: {weather.Name}");

        Console.WriteLine();
        Console.WriteLine("CURRENT WEATHER");
        Console.WriteLine("----------------------------------------");

        Console.WriteLine(
            $"Temperature : {temperature:F1} °{unit}");

        Console.WriteLine(
            $"Feels Like  : {feelsLike:F1} °{unit}");

        Console.WriteLine(
            $"Humidity    : {weather.Main.Humidity}%");

        Console.WriteLine(
            $"Pressure    : {weather.Main.Pressure} hPa");

        if (weather.Weather.Count > 0)
        {
            Console.WriteLine(
                $"Condition   : {weather.Weather[0].Description}");
        }

        Console.WriteLine(
            $"Wind Speed  : {weather.Wind.Speed:F1} m/s");

        if (weather.Main.Temperature > 35)
        {
            Console.WriteLine();
            Console.WriteLine("!!! HOT WEATHER ALERT !!!");
            Console.WriteLine("Temperature is above 35 °C.");
        }

        Console.WriteLine();
        Console.WriteLine("FORECAST");
        Console.WriteLine("----------------------------------------");

        bool rainAlertShown = false;

        foreach (ForecastItemDto item in forecast.Items)
        {
            string condition =
                item.Weather.Count > 0
                    ? item.Weather[0].Description
                    : "Unknown";

            decimal rain =
                item.RainProbability * 100;

            decimal forecastTemperature =
                ConvertTemperature(
                    item.Main.Temperature,
                    unit);

            Console.WriteLine(
                $"{item.DateTimeText}  " +
                $"{forecastTemperature:F1} °{unit}  " +
                $"{condition}  " +
                $"{rain:F0}%");

            if (rain >= 60)
            {
                rainAlertShown = true;
            }
        }

        Console.WriteLine();

        if (rainAlertShown)
        {
            Console.WriteLine("!!! RAIN ALERT !!!");
            Console.WriteLine(
                "There is a 60% or higher chance of rain.");
        }

        Console.WriteLine();
        Console.WriteLine("FORECAST SUMMARY");
        Console.WriteLine("----------------------------------------");

        decimal highestTemperature =
            forecast.Items.Max(
                item => item.Main.Temperature);

        decimal lowestTemperature =
            forecast.Items.Min(
                item => item.Main.Temperature);

        decimal averageTemperature =
            forecast.Items.Average(
                item => item.Main.Temperature);

        decimal highestRainProbability =
            forecast.Items.Max(
                item => item.RainProbability) * 100;

        highestTemperature =
            ConvertTemperature(
                highestTemperature,
                unit);

        lowestTemperature =
            ConvertTemperature(
                lowestTemperature,
                unit);

        averageTemperature =
            ConvertTemperature(
                averageTemperature,
                unit);

        Console.WriteLine(
            $"Highest Temperature : {highestTemperature:F1} °{unit}");

        Console.WriteLine(
            $"Lowest Temperature  : {lowestTemperature:F1} °{unit}");

        Console.WriteLine(
            $"Average Temperature : {averageTemperature:F1} °{unit}");

        Console.WriteLine(
            $"Highest Rain Chance : {highestRainProbability:F0}%");
    }
}