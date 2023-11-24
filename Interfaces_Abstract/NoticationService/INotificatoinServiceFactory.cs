using StorageApp.Interfaces_Abstract.NoticationService;

namespace StorageApp.Interfaces
{
    public interface INotificatoinServiceFactory
    {
        IPushNotificationStrategy GetNotificatoinProvider(string provider);
    }
}
