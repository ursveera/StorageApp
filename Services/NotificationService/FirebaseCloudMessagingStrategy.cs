using Amazon.SimpleNotificationService.Model;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using StorageApp.Interfaces_Abstract.NoticationService;
using StorageApp.Models;
using StorageApp.Options.CloudConfig;
using System.Net;

namespace StorageApp.Services.NotificationService
{
    public class FirebaseCloudMessagingStrategy : IPushNotificationStrategy
    {
        private readonly NotificationServiceOptions? _notificationServiceOptions;
        private readonly string configFilePath;
        private readonly GoogleCredential googlecredential;

        public FirebaseCloudMessagingStrategy(NotificationServiceOptions? notificationServiceOptions)
        {
            _notificationServiceOptions = notificationServiceOptions;
            configFilePath = Path.Combine(Directory.GetCurrentDirectory(), notificationServiceOptions.gcp.apiconfigfilejson);
            if (FirebaseApp.DefaultInstance == null)
            {
                googlecredential = GoogleCredential.FromFile(configFilePath);
                FirebaseApp.Create(new AppOptions
                {
                    Credential = googlecredential,
                });
            }
        }

        public async  Task SendNotificationAsync(string message)
        {
            try
            {

            var notificationMessage = new Message
            {
                Topic = "NotifcationServiceTest",
                Notification = new Notification
                {
                    Title = "Your Notification Title",
                    Body = message
                },
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(notificationMessage);

            }
            catch (Exception ex) 
            {

                throw;
            }
        }
    }
}
