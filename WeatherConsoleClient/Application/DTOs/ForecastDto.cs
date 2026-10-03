using System.Text.Json.Serialization;

namespace WeatherConsoleClient.Application.DTOs;

public class ForecastDto
{
    [JsonPropertyName("list")]
    public List<ForecastItemDto> Items { get; set; } = new();

    [JsonPropertyName("city")]
    public ForecastCityDto City { get; set; } = new();
}