using Azure;
using Google.Api.Gax.ResourceNames;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using StorageApp.Models.APIResponse;
using System.Drawing;
using System.Xml.Serialization;

namespace StorageApp.Services
{
    public class AzureFileStorageService : ICloudStorageService
    {
        private readonly CloudOptions? cloudoptions;
        private readonly string storageConnectionString;
        private readonly string containerName;
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudBlobClient blobClient;
        private readonly CloudBlobContainer blobContainer;
        public AzureFileStorageService(CloudOptions options)
        {
            cloudoptions = options;
            storageConnectionString = cloudoptions.Azure.ConnectionString;
            containerName = cloudoptions.Azure.BlobContainerName;
            CloudStorageAccount.TryParse(storageConnectionString, out storageAccount);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(containerName);
        }

        public async Task<byte[]> DownloadFileAsync(string filename)
        {
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(filename);
            var memoryStream = new MemoryStream();
            await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
            memoryStream.Position = 0;
            return memoryStream.ToArray();
        }

        public async Task<List<FileInformation>> ListAllFileAsync(string prefix)
        {
            List<FileInformation> fileInformation = new List<FileInformation>();
            var blobs = await blobContainer.ListBlobsSegmentedAsync(null);
            foreach (var blob in blobs.Results)
            {
                if (blob is CloudBlockBlob blockBlob)
                {
                    var fileInfo = new FileInformation
                    {
                        ID = Guid.NewGuid().ToString(),
                        fileName = blockBlob.Name,
                        fileType = Path.GetExtension(blockBlob.Name),
                        createdOn = blockBlob.Properties.Created.ToString(),
                        createdBy = "Admin",
                        access = "Private",
                        size = (ulong)blockBlob.Properties.Length
                    };
                    fileInformation.Add(fileInfo);
                }
            }
            return fileInformation;
        }

        public async Task UploadFileAsync(string filename, byte[] content)
        {
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(filename);
            await cloudBlockBlob.UploadFromByteArrayAsync(content, 0, content.Length);
        }

        public async Task<FilesList> ListAllFileAndFoldersAsync(string prefix)
        {
            var directory = blobContainer.GetDirectoryReference(prefix);
            BlobContinuationToken continuationToken = null;
            List<FolderInformation> folderinformationList = new List<FolderInformation>();
            var filesList = new FilesList
            {
                fileInfo = new List<FileInformation>(),
                folderInfo = new List<FolderInformation>()
            };
            var results = new List<IListBlobItem>();
            do
            {
                var response = await directory.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            }
            while (continuationToken != null);

            List<FileInformation> fileInformations = new List<FileInformation>();

            foreach (var blob in results)
            {
                string blobType = string.Empty;
                if (blob is CloudBlockBlob blockBlob)
                {
                    filesList.fileInfo.Add(new FileInformation
                    {
                        ID = Guid.NewGuid().ToString(),
                        fileName = Path.GetFileName(blockBlob.Name),
                        fileType = Path.GetExtension(blockBlob.Name),
                        createdOn = blockBlob.Properties.Created.ToString(),
                        createdBy = "Admin",
                        access = "Private",
                        size = (ulong)blockBlob.Properties.Length
                    });
                }
                else
                {
                    var blobDir = blob as CloudBlobDirectory;
                    string foldername = string.Empty;
                    if (prefix != "")
                    {
                        foldername = blobDir.Prefix.Replace(prefix, "");
                    }
                    else foldername = blobDir.Prefix;
                    filesList.folderInfo.Add(new FolderInformation
                    {
                        createdBy = "Admin",
                        createdOn = "",
                        size = "",
                        folderName = foldername
                    });
                }
            }
            return filesList;
        }
    }
}
