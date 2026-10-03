using WeatherConsoleClient.Application.DTOs;
using WeatherConsoleClient.Application.Interfaces;

namespace WeatherConsoleClient.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherApiClient _apiClient;

    public WeatherService(IWeatherApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(
        string city,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required.");
        }

        return await _apiClient.GetCurrentWeatherAsync(
            city,
            cancellationToken);
    }

    public async Task<ForecastDto?> GetForecastAsync(
        string city,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City is required.");
        }

        return await _apiClient.GetForecastAsync(
            city,
            cancellationToken);
    }
}