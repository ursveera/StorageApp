using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
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
using System;
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
            string[] folders = folderpath.Split('/');
            string currentPath = string.Empty;
            foreach (string folder in folders)
            {
                if (!string.IsNullOrEmpty(folder))
                {
                    currentPath += folder + "/";
                    await storage.UploadObjectAsync(cloudoptions.Gcp.bucketname, currentPath, "application/x-www-form-urlencoded", new System.IO.MemoryStream());
                }
            }
        }

        public async Task DeleteFileAsync(string filename)
        {
            foreach (var storageObject in storage.ListObjects(cloudoptions.Gcp.bucketname, filename))
            {
                await storage.DeleteObjectAsync(storageObject);
            }
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
            var listOptions = new ListObjectsOptions { Delimiter = "/" };
            var objects = storage.ListObjectsAsync(cloudoptions.Gcp.bucketname);
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
                string currentFileName = prefix + Path.GetFileName(storageObject.Name);
                if (Path.GetDirectoryName(storageObject.Name) == "" && prefix != "")
                {
                    continue;
                }
                else if (storageObject.Name == currentFileName && Path.GetExtension(storageObject.Name) != "")
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
                    if ((prefix == "" && Path.GetDirectoryName(storageObject.Name).Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)||(prefix != "" && Path.GetDirectoryName(storageObject.Name).Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Length == prefix.Split('/').Length && storageObject.Name.StartsWith(prefix)))
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

                //else
                //{
                //    if (prefixSlashCount >= 1)
                //    {
                //        folderinformationList.Add(new FolderInformation
                //        {
                //            createdBy = "Admin",
                //            createdOn = storageObject.TimeCreated.ToString(),
                //            size = storageObject.Size.ToString(),
                //            folderName = storageObject.Name
                //        });
                //        filesList.folderInfo = folderinformationList;
                //    }
                //}

            }
            filesList.folderInfo = folderinformationList;
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
            try
            {

                using (MemoryStream stream = new MemoryStream(content))
                {
                    if (Path.GetDirectoryName(filename) != "")
                    {
                        var storageObject = storage.GetObject(cloudoptions.Gcp.bucketname, Path.GetDirectoryName(filename));
                    }
                    await storage.UploadObjectAsync(cloudoptions.Gcp.bucketname, filename, MimeMapping.GetContentTypeFromExtension(filename), stream);
                }
            }
            catch (Google.GoogleApiException e) when (e.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                CreateFolderAsync(Path.GetDirectoryName(filename).Replace("\\", "/"));
                using (MemoryStream stream = new MemoryStream(content))
                {
                    await storage.UploadObjectAsync(cloudoptions.Gcp.bucketname, filename, MimeMapping.GetContentTypeFromExtension(filename), stream);
                }
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
