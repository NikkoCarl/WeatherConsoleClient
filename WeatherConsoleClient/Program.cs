using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherConsoleClient.Application.Interfaces;
using WeatherConsoleClient.Application.Services;
using WeatherConsoleClient.Configuration;
using WeatherConsoleClient.Infrastructure.Api;
using WeatherConsoleClient.Presentation;

class Program
{
    static async Task Main(string[] args)
    {
        IConfiguration configuration =
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: false)
                .Build();

        ServiceCollection services =
            new ServiceCollection();

        services.Configure<OpenWeatherOptions>(
            configuration.GetSection("OpenWeather"));

        services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(
            client =>
            {
                client.BaseAddress =
                    new Uri(
                        configuration["OpenWeather:BaseUrl"]!);
            });

        services.AddTransient<
            IWeatherService,
            WeatherService>();

        services.AddTransient<ConsoleMenu>();

        ServiceProvider serviceProvider =
            services.BuildServiceProvider();

        ConsoleMenu menu =
            serviceProvider.GetRequiredService<ConsoleMenu>();

        await menu.RunAsync(
            CancellationToken.None);
    }
}