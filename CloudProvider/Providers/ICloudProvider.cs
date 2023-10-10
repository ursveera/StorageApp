

using StorageApp.CloudProvider.Config;
using StorageApp.Models;

namespace StorageApp.CloudProvider.Providers
{
    public interface ICloudProvider
    {
        string GenerateCloudUrl(CloudOptions cloudOptions, Storage storage);
    }
}
