using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;

namespace StorageApp.Interfaces
{
    public interface IRDBMSConfiguration
    {
        Task<Object> GetRDBMSConfig(string cloud);
        void UpdateRDBMSSettings(RDBMSOptions updatedSettings);
        void UpdateRDBMSSettings(string  target);
    }
}
