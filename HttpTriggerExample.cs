using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Jukia.Function;

public class HttpTriggerExample
{
    private readonly ILogger<HttpTriggerExample> _logger;

    public HttpTriggerExample(ILogger<HttpTriggerExample> logger)
    {
        _logger = logger;
    }

    [Function("HttpTriggerExample")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var name = req.Query["name"].ToString();

        // If no query parameter is provided, try JSON body: { "name": "..." }
        if (string.IsNullOrWhiteSpace(name) && req.ContentLength > 0)
        {
            using var reader = new StreamReader(req.Body);
            var rawBody = await reader.ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(rawBody))
            {
                try
                {
                    using var json = JsonDocument.Parse(rawBody);
                    if (json.RootElement.TryGetProperty("name", out var nameProperty))
                    {
                        name = nameProperty.GetString() ?? string.Empty;
                    }
                }
                catch (JsonException)
                {
                    return new BadRequestObjectResult("Invalid JSON. Send something like: { \"name\": \"Jukia\" }");
                }
            }
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return new OkObjectResult("Function running! Add ?name=Jukia to the URL or send JSON { \"name\": \"Jukia\" }");
        }

        return new OkObjectResult($"Hello, {name}! Your Azure Function is working.");
    }
}
