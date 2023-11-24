using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json;
using StorageApp.Interfaces_Abstract.NoticationService;
using StorageApp.Models.NotificationService;
using StorageApp.Options.CloudConfig;
using Notification = StorageApp.Models.NotificationService.Notification;

namespace StorageApp.Services.NotificationService
{
    public class AzureNotificationHubsStrategy : IPushNotificationStrategy
    {
        private readonly string _connectionString;
        private readonly string _hubName;
        private readonly NotificationServiceOptions? notificationServiceOptions;
        public AzureNotificationHubsStrategy(NotificationServiceOptions options)
        {
            notificationServiceOptions = options;
            _connectionString = notificationServiceOptions.azure.NotificationListenManageSendConnectionString;
            _hubName = notificationServiceOptions.azure.NotificationHubName;
        }
        public async Task SendNotificationAsync(string message)
        {
            var hubClient = NotificationHubClient.CreateClientFromConnectionString(_connectionString, _hubName);
            var payload = new NotificationPayload
            {
                Notification = new Notification
                {
                    Title = "Notification Hub Test Notification",
                    Body = message
                },
                Data = new Dictionary<string, object>
            {
                { "property1", "value1" },
                { "property2", 42 }
            }
            };
            var outcome = await hubClient.SendFcmNativeNotificationAsync(JsonConvert.SerializeObject(payload));

        }
    }
}


