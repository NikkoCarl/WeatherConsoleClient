using System.Text.Json.Serialization;

namespace WeatherConsoleClient.Application.DTOs;

public class WeatherConditionDto
{
    [JsonPropertyName("main")]
    public string Main { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}