using StorageApp.Options.CloudConfig;

namespace StorageApp.Interfaces_Abstract.NoticationService
{
    public interface INotificationServiceConfiguration
    {
        Task<Object> GetNotificationServiceConfig(string cloudname);
        void UpdateNotificationService(NotificationServiceOptions options);
    }
}
