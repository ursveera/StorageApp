using StorageApp.CloudProvider.Providers;

namespace StorageApp.CloudProvider.Factory
{
    public interface ICloudProviderFactory
    {
        ICloudProvider GetCloudProvider(string target);
    }
}
