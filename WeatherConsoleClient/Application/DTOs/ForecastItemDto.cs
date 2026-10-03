using System.Text.Json.Serialization;

namespace WeatherConsoleClient.Application.DTOs;

public class ForecastItemDto
{
    [JsonPropertyName("dt_txt")]
    public string DateTimeText { get; set; } = "";

    [JsonPropertyName("main")]
    public ForecastMainDto Main { get; set; } = new();

    [JsonPropertyName("weather")]
    public List<WeatherConditionDto> Weather { get; set; } = new();

    [JsonPropertyName("pop")]
    public decimal RainProbability { get; set; }
}