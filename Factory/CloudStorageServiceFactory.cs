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
        private readonly IServiceProvider _serviceProvider;
        private readonly CloudOptions _options;
        public CloudStorageServiceFactory(IServiceProvider serviceProvider,IOptionsMonitor<CloudOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options.CurrentValue;
        }
        public ICloudStorageService GetFileStorageService(string provider)
        {
            //switch (_options.Target.ToLower())
            //{
            //    case "azure":
            //        string azureBlobUrl = Path.Combine(_options.Azure.StorageUrl, _options.Azure.BlobContainerName, "{filename}" + "?" + _options.Azure.SASToken);
            //        List<ContentHeader> azureUploadheaders = new List<ContentHeader>() { new ContentHeader() { Key = "x-ms-blob-type", Value = "BlockBlob" } };
            //        return new CommonCloudStorageService().WithBaseUrl(azureBlobUrl).WithUploadHeaders(azureUploadheaders);
            //    case "aws":
            //        string awsS3Url = Path.Combine(_options.AWS.uploadUrl, _options.AWS.Stage, _options.AWS.BucketName, "{filename}");
            //        List<ContentHeader> awsUploadheader = new List<ContentHeader>() { new ContentHeader() { Key = "Content-Type", Value = MediaTypeNames.Application.Octet } };
            //        List<ContentHeader> awsDownloadheader = new List<ContentHeader>() { new ContentHeader() { Key = "x-api-key", Value = _options.AWS.XAPIKEY } };
            //        return new CommonCloudStorageService().WithBaseUrl(awsS3Url).WithUploadHeaders(awsUploadheader).WithDownloadHeaders(awsDownloadheader);
            //    default:
            //        throw new ArgumentException("Invalid file storage provider.");
            //}
            switch (provider.ToLower())
            {
                case "aws":
                    return new AWSFileStorageService(_options); // Create AWS-specific service.
                case "azure":
                    return new AzureFileStorageService(_options); // Create Azure-specific service.
                default:
                    throw new ArgumentException("Invalid file storage provider.");
            }
        }
    }
}
