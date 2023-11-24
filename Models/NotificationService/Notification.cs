using Newtonsoft.Json;

namespace StorageApp.Models.NotificationService
{
    public class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class NotificationPayload
    {
        [JsonProperty("notification")]
        public Notification Notification { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
    }
}
