namespace WeatherConsoleClient.Configuration;

public class OpenWeatherOptions
{
    public string BaseUrl { get; set; } = "";

    public string ApiKey { get; set; } = "";

    public string Units { get; set; } = "metric";
}