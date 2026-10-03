Why didn't we simply put all of this inside Program.cs?

We did not put everything in Program.cs because it would be too long and confusing. We put the code in different files. Each file has one job. This makes the program easy to read, test, and fix.

PART 30 — Design Questions

Question 1 — HTTP

What HTTP method did we use?

We used GET.

Why is GET appropriate for retrieving weather information?

GET is used to get information. We use GET to get weather information from the API.

Question 2 — JSON

Why does the API return JSON instead of a C# object?

The API uses JSON because it is easy for computers to send and read.

What process converts JSON into our C# DTO?

Our program uses JSON deserialization. It changes JSON data into our C# DTO.

Question 3 — DTO

Why did we create CurrentWeatherDto, ForecastDto, and ForecastItemDto instead of directly working with raw JSON?

We made DTOs to make the weather data easy to use. They give the data a clear form. This is easier than reading raw JSON.

Question 4 — Configuration

Why is the API key stored in appsettings.json instead of Program.cs?

We put the API key in appsettings.json to keep it separate from the code. It is also easy to change. This helps keep the program clean and safe.

Question 9 — Error Handling

1. What happens if the city does not exist?

The program says "City not found."

2. What happens if the API key is invalid?

The API gives an error. The program catches the error and shows an error message.

3. What happens if there is no Internet connection?

The request fails. The program catches the error and shows an API error message.

4. What happens if the API is unavailable?

The program catches the error and shows an error message.

5. What happens if the user presses Ctrl+C?

The program can cancel the request. It then shows "Operation cancelled."

6. What happens if the API returns an unexpected response?

The program catches the error and shows "Something went wrong."

This helps stop the program from crashing.