using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Azure;
using Google.Api.Gax.ResourceNames;
using Google.Apis.Storage.v1.Data;
using Microsoft.Extensions.Options;
using RestSharp;
using StorageApp.CloudProvider.Config;
using StorageApp.Extensions;
using StorageApp.Models;
using StorageApp.Models.APIResponse;
using System.Net.Mime;
using System.Xml.Serialization;

namespace StorageApp.Services
{
    public class AWSFileStorageService : ICloudStorageService
    {
        private readonly string BaseUrl;
        private readonly List<ContentHeader>? UploadHeaders;
        private List<ContentHeader>? DownloadHeaders;
        private readonly CloudOptions? cloudoptions;

        private readonly string accessKey;
        private readonly string secretKey;
        private readonly RegionEndpoint region = RegionEndpoint.USEast2;
        private readonly AmazonS3Client s3Client;
        public AWSFileStorageService(CloudOptions options)
        {
            cloudoptions = options;
            BaseUrl = Path.Combine(cloudoptions.AWS.uploadUrl, cloudoptions.AWS.Stage, cloudoptions.AWS.BucketName, "{filename}");
            DownloadHeaders = new List<ContentHeader>() { new ContentHeader() { Key = "x-api-key", Value = cloudoptions.AWS.XAPIKEY } };
            accessKey = cloudoptions.AWS.AccessKey;
            secretKey = cloudoptions.AWS.SecretKey;
            s3Client = new AmazonS3Client(accessKey, secretKey, region);
        }

        public async Task<byte[]> DownloadFileAsync(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = cloudoptions.AWS.BucketName,
                Key = fileName
            };
            using var response = await s3Client.GetObjectAsync(request);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<List<FileInformation>> ListAllFileAsync(string prefix)
        {
            var request = new ListObjectsRequest
            {
                BucketName = cloudoptions.AWS.BucketName,
                Prefix = prefix
            };
            var response = await s3Client.ListObjectsAsync(request);
            var fileInformation = response.S3Objects.Select(e => new FileInformation
            {
                fileName = e.Key,
                createdOn = e.LastModified.ToString(),
                fileType = Path.GetExtension(e.Key),
                access = e.Owner.Id,
                createdBy = "Admin",
                size = (ulong)e.Size
            }).ToList();
            return fileInformation;
        }

        public async Task UploadFileAsync(string filename, byte[] bytecontent)
        {
            using (MemoryStream stream = new MemoryStream(bytecontent))
            {
                if (Path.GetDirectoryName(filename) != "")
                {
                    CreateFolderAsync(Path.GetDirectoryName(filename).Replace("\\", "/"));
                }
            }
            var pubObjectrequest = new PutObjectRequest
            {
                BucketName = cloudoptions.AWS.BucketName,
                Key = filename,
                InputStream = new MemoryStream(bytecontent)
            };
            var response = await s3Client.PutObjectAsync(pubObjectrequest);
        }


        public async Task<FilesList> ListAllFileAndFoldersAsync(string prefix)
        {
            List<FileInformation> fileInformationList = new List<FileInformation>();
            List<FolderInformation> folderinformationList = new List<FolderInformation>();
            List<ListObjectsResponse> s3Objects = new List<ListObjectsResponse>();
            string storageName = "";
            var request = new ListObjectsV2Request
            {
                BucketName = cloudoptions.AWS.BucketName,
                Prefix = prefix
            };
            var response = await s3Client.ListObjectsV2Async(request);
            var filesList = new FilesList
            {
                fileInfo = new List<FileInformation>(),
                folderInfo = new List<FolderInformation>()
            };
            response.S3Objects.ForEach(e =>
            {
                if (prefix != "") storageName = e.Key.Replace(prefix, "");
                else storageName = e.Key;
                if (Path.GetDirectoryName(storageName) == "")
                {
                    storageName = Path.GetFileName(e.Key);
                    filesList.fileInfo.Add(new FileInformation
                    {
                        fileName = storageName,
                        fileType = e.Key.Substring(e.Key.LastIndexOf('.') + 1),
                        createdOn = e.LastModified.ToString() ?? "",
                        createdBy = "Admin",
                        access = e.Owner?.Id,
                        size = (ulong)e.Size,
                    });
                }
                else
                {
                    char target = '/';
                    int prefixSlashCount = prefix.Count(e => e == target);
                    if (e.Key.Count(e => e == target) == prefixSlashCount + 1 && Path.GetExtension(e.Key) == "")
                    {
                        int index = e.Key.IndexOf(prefix);
                        if (index >= 0)
                        {
                            string foldername = e.Key.Remove(index, prefix.Length).Insert(index, "");
                            folderinformationList.Add(new FolderInformation
                            {
                                createdBy = "Admin",
                                createdOn = e.LastModified.ToString(),
                                size = e.Size.ToString(),
                                folderName = foldername
                            });
                            filesList.folderInfo = folderinformationList;
                        }
                    }
                }
            });
            return filesList;

        }

        public async Task DeleteFileAsync(string filename)
        {
            var listRequest = new ListObjectsV2Request
            {
                BucketName = cloudoptions.AWS.BucketName,
                Prefix = filename
            };
            ListObjectsV2Response listResponse = await s3Client.ListObjectsV2Async(listRequest);
            foreach (var s3Object in listResponse.S3Objects)
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = cloudoptions.AWS.BucketName,
                    Key = s3Object.Key
                };
                await s3Client.DeleteObjectAsync(deleteRequest);
            }
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
                    var foldercreate = new PutObjectRequest
                    {
                        BucketName = cloudoptions.AWS.BucketName,
                        Key = currentPath
                    };
                    var foldercreateresponse = await s3Client.PutObjectAsync(foldercreate);
                }
            }
        }

        public async Task<bool> CheckExists(string filename)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = cloudoptions.AWS.BucketName,
                    Key = filename
                };
                using var response = await s3Client.GetObjectAsync(request);
                using (var memoryStream = new MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(memoryStream);
                    if (memoryStream.ToArray().Length <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Amazon.S3.AmazonS3Exception e)
            {
                return false;
            }
        }
    }
}
