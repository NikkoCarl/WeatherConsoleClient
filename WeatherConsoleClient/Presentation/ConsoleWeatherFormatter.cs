using System.Text;
using WeatherConsoleClient.Application.DTOs;
using WeatherConsoleClient.Application.Interfaces;

namespace WeatherConsoleClient.Presentation;

public class ConsoleWeatherFormatter : IWeatherFormatter
{
    public string FormatCurrentWeather(
        CurrentWeatherDto weather)
    {
        StringBuilder result = new StringBuilder();

        result.AppendLine();
        result.AppendLine("========================================");
        result.AppendLine("CURRENT WEATHER");
        result.AppendLine("========================================");

        result.AppendLine(
            $"City        : {weather.Name}");

        result.AppendLine(
            $"Temperature : {weather.Main.Temperature:F2} °C");

        result.AppendLine(
            $"Feels Like  : {weather.Main.FeelsLike:F2} °C");

        result.AppendLine(
            $"Humidity    : {weather.Main.Humidity}%");

        result.AppendLine(
            $"Pressure    : {weather.Main.Pressure} hPa");

        if (weather.Weather.Count > 0)
        {
            result.AppendLine(
                $"Condition   : {weather.Weather[0].Description}");
        }

        result.AppendLine(
            $"Wind Speed  : {weather.Wind.Speed:F2} m/s");

        return result.ToString();
    }

    public string FormatForecast(
        ForecastDto forecast)
    {
        StringBuilder result = new StringBuilder();

        result.AppendLine();
        result.AppendLine("========================================");
        result.AppendLine("5-DAY / 3-HOUR FORECAST");
        result.AppendLine("========================================");

        result.AppendLine();
        result.AppendLine(
            $"City: {forecast.City.Name}");

        result.AppendLine();

        foreach (ForecastItemDto item in forecast.Items)
        {
            string condition =
                item.Weather.Count > 0
                    ? item.Weather[0].Description
                    : "Unknown";

            decimal rain =
                item.RainProbability * 100;

            result.AppendLine(
                $"{item.DateTimeText}  " +
                $"{item.Main.Temperature:F1} °C  " +
                $"{condition}  " +
                $"{rain:F0}%");
        }

        return result.ToString();
    }
}