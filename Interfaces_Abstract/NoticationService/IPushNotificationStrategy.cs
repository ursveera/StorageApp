namespace StorageApp.Interfaces_Abstract.NoticationService
{
    public interface IPushNotificationStrategy
    {
        Task SendNotificationAsync(string message);
    }
}
