using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WeatherConsoleClient.Application.DTOs;
using WeatherConsoleClient.Application.Interfaces;
using WeatherConsoleClient.Configuration;

namespace WeatherConsoleClient.Infrastructure.Api;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _options;

    public WeatherApiClient(
        HttpClient httpClient,
        IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(
        string city,
        CancellationToken cancellationToken)
    {
        string url =
            $"data/2.5/weather?q={Uri.EscapeDataString(city)}" +
            $"&appid={_options.ApiKey}" +
            $"&units={_options.Units}";

        HttpResponseMessage response =
            await _httpClient.GetAsync(
                url,
                cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        string json =
            await response.Content.ReadAsStringAsync(
                cancellationToken);

        return JsonSerializer.Deserialize<CurrentWeatherDto>(json);
    }

    public async Task<ForecastDto?> GetForecastAsync(
        string city,
        CancellationToken cancellationToken)
    {
        string url =
            $"data/2.5/forecast?q={Uri.EscapeDataString(city)}" +
            $"&appid={_options.ApiKey}" +
            $"&units={_options.Units}";

        HttpResponseMessage response =
            await _httpClient.GetAsync(
                url,
                cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        string json =
            await response.Content.ReadAsStringAsync(
                cancellationToken);

        return JsonSerializer.Deserialize<ForecastDto>(json);
    }
}