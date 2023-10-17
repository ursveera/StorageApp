using Amazon.S3.Model;
using Google.Api.Gax.ResourceNames;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using System.IO;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text.Json;

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
            configFilePath = Path.Combine(Directory.GetCurrentDirectory(), options.Gcp.apiconfigfilejson);
            googlecredential = GoogleCredential.FromFile(configFilePath);
            storage = StorageClient.Create(googlecredential);
        }



        public async Task CreateFolderAsync(string folderpath)
        {
            await storage.UploadObjectAsync(cloudoptions.Gcp.bucketname, folderpath, "application/x-www-form-urlencoded", new System.IO.MemoryStream());
        }

        public async Task DeleteFileAsync(string filename)
        {
            await storage.DeleteObjectAsync(cloudoptions.Gcp.bucketname, filename);
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

        public async Task<FilesList> ListAllFileAndFoldersAsync(string prefix)
        {
            var objects = storage.ListObjectsAsync(cloudoptions.Gcp.bucketname, prefix);
            List<FileInformation> fileInformationList = new List<FileInformation>();
            List<FolderInformation> folderinformationList = new List<FolderInformation>();
            List<Google.Apis.Storage.v1.Data.Object> objectss = new List<Google.Apis.Storage.v1.Data.Object>();
            string storageName = "";
            var filesList = new FilesList
            {
                fileInfo = new List<FileInformation>(),
                folderInfo = new List<FolderInformation>()
            };
            await foreach (var storageObject in objects)
            {
                objectss.Add(storageObject);
            }
            foreach (var storageObject in objectss)
            {
                if (prefix != "") storageName = storageObject.Name.Replace(prefix, "");
                else storageName = storageObject.Name;
                if (Path.GetDirectoryName(storageName) == "")
                {
                    storageName = Path.GetFileName(storageObject.Name);
                    filesList.fileInfo.Add(new FileInformation
                    {
                        fileName = storageName,
                        fileType = storageObject.Name.Substring(storageObject.Name.LastIndexOf('.') + 1),
                        createdOn = storageObject.TimeCreated?.ToString() ?? "",
                        createdBy = "Admin",
                        access = "Full",
                        size = storageObject.Size,
                    });
                }
                else
                {
                    char target = '/';
                    int prefixSlashCount = prefix.Count(e => e == target);
                    if (storageObject.Name.Count(e => e == target) == prefixSlashCount + 1 && Path.GetExtension(storageObject.Name) == "")
                    {
                        int index = storageObject.Name.IndexOf(prefix);
                        if (index >= 0)
                        {
                            string foldername = storageObject.Name.Remove(index, prefix.Length).Insert(index, "");
                            folderinformationList.Add(new FolderInformation
                            {
                                createdBy = "Admin",
                                createdOn = storageObject.TimeCreated.ToString(),
                                size = storageObject.Size.ToString(),
                                folderName = foldername
                            });
                            filesList.folderInfo = folderinformationList;
                        }
                    }
                }
            }
            return filesList;
        }

        public async Task<List<FileInformation>> ListAllFileAsync(string prefix)
        {
            var objects = storage.ListObjectsAsync(cloudoptions.Gcp.bucketname, prefix);
            List<FileInformation> fileInformationList = new List<FileInformation>();
            await foreach (var storageObject in objects)
            {
                var fileInformation = new FileInformation
                {
                    fileName = storageObject.Name,
                    createdBy = "Admin",
                    createdOn = storageObject.TimeCreated.ToString(),
                    fileType = Path.GetExtension(storageObject.Name),
                    access = "Full",
                    size = storageObject.Size
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
        public async Task<bool> CheckExists(string filename)
        {
            try
            {
                var file = await storage.GetObjectAsync(cloudoptions.Gcp.bucketname, filename);
                if (string.IsNullOrEmpty(file.Name))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Google.GoogleApiException e)
            {
                return false;
            }
        }
    }
}
