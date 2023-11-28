using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using StorageApp.CloudProvider.Config;
using StorageApp.Interfaces_Abstract.NoticationService;
using StorageApp.Options.CloudConfig;

namespace StorageApp.Services.NotificationService
{
    public class AwsSnsNotificationStrategy: IPushNotificationStrategy
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _arn;
        private readonly RegionEndpoint _region;
        private readonly NotificationServiceOptions? notificationServiceOptions;

        public AwsSnsNotificationStrategy(NotificationServiceOptions options)
        {
            notificationServiceOptions = options;
            _accessKey = notificationServiceOptions.aws.AccessKey;
            _secretKey = notificationServiceOptions.aws.SecretKey;
            _region = RegionEndpoint.USEast2;
            _arn = notificationServiceOptions.aws.Arn;
        }

        public async Task SendNotificationAsync(string message)
        {
            using var snsClient = new AmazonSimpleNotificationServiceClient(_accessKey, _secretKey, _region);
            var request = new PublishRequest
            {
                //TopicArn = "arn:aws:sns:us-east-2:547735170130:NotifcationServiceTest",
                Message = message,
                TargetArn = _arn  //for single device
            };
            var response = await snsClient.PublishAsync(request);

        }
    }
}
