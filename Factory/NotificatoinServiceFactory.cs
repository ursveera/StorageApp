using Microsoft.Extensions.Options;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract.NoticationService;
using StorageApp.Models;
using StorageApp.Options.CloudConfig;
using StorageApp.Services;
using StorageApp.Services.NotificationService;
using System.Net.Mime;

namespace StorageApp.Factory
{
    public class NotificatoinServiceFactory : INotificatoinServiceFactory
    {
        private readonly NotificationServiceOptions _options;
        public NotificatoinServiceFactory(IServiceProvider serviceProvider,IOptionsMonitor<NotificationServiceOptions> options)
        {
            _options = options.CurrentValue;
        }
        public IPushNotificationStrategy GetNotificatoinProvider(string provider)
        {
            switch (provider.ToLower())
            {
                case "aws":
                    return new AwsSnsNotificationStrategy(_options); 
                case "azure":
                    return new AzureNotificationHubsStrategy(_options);
                case "gcp":
                    return new FirebaseCloudMessagingStrategy(_options);
                default:
                    throw new ArgumentException("Invalid file storage provider.");
            }
        }
    }
}
