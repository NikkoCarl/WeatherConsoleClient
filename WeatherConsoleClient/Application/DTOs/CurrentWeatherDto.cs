using System.Text.Json.Serialization;

namespace WeatherConsoleClient.Application.DTOs;

public class CurrentWeatherDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("main")]
    public CurrentWeatherMainDto Main { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherConditionDto> Weather { get; set; } = new();

    [JsonPropertyName("wind")]
    public WindDto Wind { get; set; } = new();
}