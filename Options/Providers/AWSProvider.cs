using StorageApp.CloudProvider.Config;
using StorageApp.Models;

namespace StorageApp.CloudProvider.Providers
{
    public class AWSProvider: ICloudProvider
    {
        public string GenerateCloudUrl(CloudOptions cloudOptions, Storage storage)
        {
            if (!string.IsNullOrEmpty(cloudOptions.AWS.Stage) && !string.IsNullOrEmpty(cloudOptions.AWS.uploadUrl))
            {
                return cloudOptions.AWS.uploadUrl
                    .Replace("{stage}", cloudOptions.AWS.Stage)
                    .Replace("{bucketname}", storage.bucket_or_containername)
                    .Replace("{filename}", storage.filename_or_blobname);
            }
            return null;
        }
    }
}
