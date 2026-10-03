using WeatherConsoleClient.Application.DTOs;

namespace WeatherConsoleClient.Application.Interfaces;

public interface IWeatherFormatter
{
    string FormatCurrentWeather(
        CurrentWeatherDto weather);

    string FormatForecast(
        ForecastDto forecast);
}