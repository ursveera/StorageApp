using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;

namespace StorageApp.Interfaces
{
    public interface INOSQLConfiguration
    {
        Task<Object> GetRDBMSConfig(string cloud);
        void UpdateRDBMSSettings(string  target);
    }
}
