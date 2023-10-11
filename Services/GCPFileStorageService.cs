using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using System.IO;
using System.Net.Sockets;
using System.Security.AccessControl;

namespace StorageApp.Services
{

    public class GCPFileStorageService : ICloudStorageService
    {
        private readonly CloudOptions? cloudoptions;
        private readonly string configFilePath;
        private readonly GoogleCredential googlecredential;
        private StorageClient storage;
        public GCPFileStorageService(CloudOptions options)
        {
            cloudoptions = options;
            configFilePath= Path.Combine(Directory.GetCurrentDirectory(), options.Gcp.apiconfigfilejson);
            googlecredential = GoogleCredential.FromFile(configFilePath);
            storage = StorageClient.Create(googlecredential);
        }
        public async Task<byte[]> DownloadFileAsync(string filename)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await storage.DownloadObjectAsync(cloudoptions.Gcp.bucketname, filename, memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return fileBytes;
            }
        }

        public async Task<List<FileInformation>> ListAllFileAsync()
        {
            var objects=storage.ListObjectsAsync(cloudoptions.Gcp.bucketname);
            List<FileInformation> fileInformationList = new List<FileInformation>();

            await foreach (var storageObject in objects)
            {
                var fileInformation = new FileInformation
                {
                    FileName = storageObject.Name,
                    CreatedBy = "Admin",
                    CreatedOn = storageObject.TimeCreated.ToString(),
                    FileType = Path.GetExtension(storageObject.Name),
                    Access = "Full",
                    Size =MimeMapping.FormatFileSize(storageObject.Size)

                };

                fileInformationList.Add(fileInformation);
            }
            return fileInformationList;
        }

        public async Task UploadFileAsync(string filename, byte[] content)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                await storage.UploadObjectAsync(cloudoptions.Gcp.bucketname, filename, MimeMapping.GetContentTypeFromExtension(filename), stream);
            }
        }
    }
}
