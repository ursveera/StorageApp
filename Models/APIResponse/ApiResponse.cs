using System.Text.Json.Serialization;

namespace StorageApp.Models.ApiResponse
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }

}
