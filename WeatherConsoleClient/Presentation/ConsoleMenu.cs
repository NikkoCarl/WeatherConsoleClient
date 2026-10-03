using WeatherConsoleClient.Application.DTOs;
using WeatherConsoleClient.Application.Interfaces;

namespace WeatherConsoleClient.Presentation;

public class ConsoleMenu
{
    private readonly IWeatherService _weatherService;

    public ConsoleMenu(IWeatherService weatherService)
    {
        _weatherService = weatherService;
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
                    await ShowCurrentWeatherAsync(
                        cancellationToken);
                }
                else if (choice == "2")
                {
                    await ShowForecastAsync(
                        cancellationToken);
                }
                else if (choice == "3")
                {
                    await ShowDashboardAsync(
                        cancellationToken);
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
                Console.WriteLine(
                    $"API Error: {ex.Message}");
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
                Console.WriteLine(
                    "Something went wrong.");
            }

            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }
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

        CurrentWeatherDto? weather =
            await _weatherService.GetCurrentWeatherAsync(
                city,
                cancellationToken);

        if (weather == null)
        {
            Console.WriteLine("City not found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("CURRENT WEATHER");
        Console.WriteLine("========================================");

        Console.WriteLine($"City        : {weather.Name}");
        Console.WriteLine(
            $"Temperature : {weather.Main.Temperature:F2} °C");
        Console.WriteLine(
            $"Feels Like  : {weather.Main.FeelsLike:F2} °C");
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
            $"Wind Speed  : {weather.Wind.Speed:F2} m/s");
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

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine("5-DAY / 3-HOUR FORECAST");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine(
            $"City: {forecast.City.Name}");
        Console.WriteLine();

        foreach (ForecastItemDto item in forecast.Items)
        {
            string condition =
                item.Weather.Count > 0
                    ? item.Weather[0].Description
                    : "Unknown";

            decimal rain =
                item.RainProbability * 100;

            Console.WriteLine(
                $"{item.DateTimeText}  " +
                $"{item.Main.Temperature:F1} °C  " +
                $"{condition}  " +
                $"{rain:F0}%");
        }
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
            $"Temperature : {weather.Main.Temperature:F1} °C");

        Console.WriteLine(
            $"Feels Like  : {weather.Main.FeelsLike:F1} °C");

        Console.WriteLine(
            $"Humidity    : {weather.Main.Humidity}%");

        if (weather.Weather.Count > 0)
        {
            Console.WriteLine(
                $"Condition   : {weather.Weather[0].Description}");
        }

        Console.WriteLine();
        Console.WriteLine("FORECAST");
        Console.WriteLine("----------------------------------------");

        foreach (ForecastItemDto item in forecast.Items)
        {
            string condition =
                item.Weather.Count > 0
                    ? item.Weather[0].Description
                    : "Unknown";

            decimal rain =
                item.RainProbability * 100;

            Console.WriteLine(
                $"{item.DateTimeText}  " +
                $"{item.Main.Temperature:F1} °C  " +
                $"{condition}  " +
                $"{rain:F0}%");
        }
    }
}