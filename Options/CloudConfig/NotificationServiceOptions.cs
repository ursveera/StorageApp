using StorageApp.Models.NotificationService;

namespace StorageApp.Options.CloudConfig
{
    public class NotificationServiceOptions
    {
        public nAzure azure { get; set; }
        public nAWS aws { get; set; }
        public nGCP gcp { get; set; }
    }

}
