using StorageApp.CloudProvider.Config;
using StorageApp.Models;

namespace StorageApp.CloudProvider.Providers
{

    public class AzureProvider : ICloudProvider
    {
        public string GenerateCloudUrl(CloudOptions cloudOptions, Storage storage)
        {
            if (!string.IsNullOrEmpty(cloudOptions.Azure.StorageUrl))
            {
                return cloudOptions.Azure.StorageUrl
                    .Replace("{containername}", storage.bucket_or_containername)
                    .Replace("{filename}", storage.filename_or_blobname);
            }
            return null;
        }
    }
}
