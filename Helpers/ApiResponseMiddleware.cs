using StorageApp.Models.ApiResponse;
using System.Net;
using System.Text.Json;
namespace StorageApp.Helpers
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiResponseMiddleware> _logger;

        public ApiResponseMiddleware(RequestDelegate next, ILogger<ApiResponseMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stream originalBody = context.Response.Body;

            try
            {
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;
                await _next(context);

                memoryStream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(memoryStream);
                string responseBody = await reader.ReadToEndAsync();

                object? data = null;
                string? customMessage = null;

                if (context.Response.Headers.ContainsKey("Content-Disposition"))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(originalBody);
                    return;
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.OK ||
                    context.Response.StatusCode == (int)HttpStatusCode.BadRequest ||
                    context.Response.StatusCode == (int)HttpStatusCode.NotFound ||
                    context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
                {
                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        try
                        {
                            data = JsonSerializer.Deserialize<object>(responseBody);
                        }
                        catch (JsonException)
                        {
                            // The response body is not a valid JSON object
                            // In this case, we assume it's plain text
                            data = responseBody;
                        }
                    }

                    if (!string.IsNullOrEmpty(responseBody) && responseBody.TrimStart().StartsWith('{'))
                    {
                        try
                        {
                            var parsedData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                            customMessage = parsedData?.ContainsKey("Message") == true ? parsedData["Message"]?.ToString() : null;
                        }
                        catch (JsonException)
                        {
                            // The response body is not a valid JSON object, ignore the parsing error
                        }
                    }
                }

                var apiResponse = new ApiResponse<object>
                {
                    StatusCode = context.Response.StatusCode,
                    Message = customMessage ?? GetDefaultMessageForStatusCode(context.Response.StatusCode),
                    Data = data
                };
                var updatedResponseBody = JsonSerializer.Serialize(apiResponse);
                context.Response.ContentType = "application/json";
                context.Response.Body = originalBody;
                context.Response.ContentLength = null;
                if (context.Response.StatusCode != (int)HttpStatusCode.NoContent)
                {
                    await context.Response.WriteAsync(updatedResponseBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred while executing the request.");

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var apiResponse = new ApiResponse<string>
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An internal server error occurred.",
                    Data = $"Error: {ex.Message}"
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(apiResponse);
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.OK => "Success",
                (int)HttpStatusCode.NoContent => "No Content",
                (int)HttpStatusCode.BadRequest => "Bad Request",
                (int)HttpStatusCode.NotFound => "Not Found",
                (int)HttpStatusCode.InternalServerError => "Internal Server Error",
                _ => "An error occurred while processing the request."
            };
        }
    }
}
