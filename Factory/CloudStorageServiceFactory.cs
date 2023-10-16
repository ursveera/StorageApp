using Microsoft.Extensions.Options;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Interfaces;
using StorageApp.Models;
using StorageApp.Services;
using System.Net.Mime;

namespace StorageApp.Factory
{
    public class CloudStorageServiceFactory: ICloudStorageServiceFactory
    {
        private readonly CloudOptions _options;
        public CloudStorageServiceFactory(IServiceProvider serviceProvider,IOptionsMonitor<CloudOptions> options)
        {
            _options = options.CurrentValue;
        }
        public ICloudStorageService GetFileStorageService(string provider)
        {
            switch (provider.ToLower())
            {
                case "aws":
                    return new AWSFileStorageService(_options); 
                case "azure":
                    return new AzureFileStorageService(_options);
                case "gcp":
                    return new GCPFileStorageService(_options);
                case "nocloud":
                    return new NoCloudFileStorageService(_options);
                default:
                    throw new ArgumentException("Invalid file storage provider.");
            }
        }
    }
}
