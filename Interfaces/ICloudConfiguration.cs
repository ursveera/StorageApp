using StorageApp.CloudProvider.Config;

namespace StorageApp.Interfaces
{
    public interface ICloudConfiguration
    {
        Task<Object> GetCloudConfig(string cloud);
        void UpdateCloudSettings(CloudOptions updatedSettings);
        void UpdateCloudSettings(AWSOptions aws);
        void UpdateCloudSettings(AZUREOptions azure);
        void UpdateCloudSettings(string  target);
    }
}
