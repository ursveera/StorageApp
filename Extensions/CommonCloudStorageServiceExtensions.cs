using StorageApp.Models;
using StorageApp.Services;

namespace StorageApp.Extensions
{
    public static class CommonCloudStorageServiceExtensions
    {
        public static CommonCloudStorageService WithBaseUrl(this CommonCloudStorageService service, string baseUrl)
        {
            service.BaseUrl = baseUrl;
            return service;
        }

        public static CommonCloudStorageService WithUploadHeaders(this CommonCloudStorageService service, List<ContentHeader> headers)
        {
            service.UploadHeaders = headers;
            return service;
        }
        public static CommonCloudStorageService WithDownloadHeaders(this CommonCloudStorageService service, List<ContentHeader> headers)
        {
            service.DownloadHeaders = headers;
            return service;
        }
    }
}
